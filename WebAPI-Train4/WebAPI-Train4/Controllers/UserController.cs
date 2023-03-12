using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebAPI_Train4.Data;
using WebAPI_Train4.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebAPI_Train4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly AppSetting _appSettings;

        public UserController(MyDbContext myDbContext, IOptionsMonitor<AppSetting> options)
        {
            _context = myDbContext;
            _appSettings = options.CurrentValue;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> validate(LoginModel loginModel)
        {
            var user = _context.NguoiDungs.SingleOrDefault(p => p.UserName == loginModel.UserName && loginModel.Password == p.Password);
            if (user == null) // khong dung
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password"
                }); ;
            }
            // Cap token
            var token = await GenerateToken(user);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = token
            });
        }

        // Sinh ma token
        private async Task<TokenModel> GenerateToken(NguoiDung nguoiDung)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyByte = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDecription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim(JwtRegisteredClaimNames.Email, nguoiDung.Email),
                    
                    // Sub của email
                    new Claim(JwtRegisteredClaimNames.Sub, nguoiDung.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.Id.ToString()),

                    //roles
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),//TIme out
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte), SecurityAlgorithms.HmacSha512Signature)


            };

            var token = jwtTokenHandler.CreateToken(tokenDecription);

            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            // Lưu database 
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId=nguoiDung.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IsSuedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddHours(1)
            };
            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken()
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel tokenModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParameter = new TokenValidationParameters
            {
                // Tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                // Ký vào token
                ValidateIssuerSigningKey = true,
                // Sử dụng thuật toán đối xứng ứng với cái Key, sẽ tự động mã hóa, về mặt mã hóa thì phải làm được trên bit thì phải encode lại
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,
                // Tránh trường hợp nhảy ra catch
                ValidateLifetime= false // Không kiểm tra token hết hạn 
            };

            try
            {
                // Check1: coi format accesstoken có ổn không 
                var tokenInveritification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken, tokenValidateParameter, out var validatedToken);

                // check2 : khác nhau về thuật toán
                if (validatedToken is JwtSecurityToken securityToken)
                {
                    //InvarianCultureIgnoreCase không phân biệt hoa thường
                    var result = securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if(!result)//false
                    {
                        return Ok( new ApiResponse
                        {
                            Success = false,
                            Message = "invalid Token"
                        });
                    }    
                }
                // check 3: access Token đã expire hay chưa
                var utcExpireDate = long.Parse(tokenInveritification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
               if(expireDate > DateTime.UtcNow)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message="Access token has not yet expire"
                    });
                }
                //checked 4: Check refreshToken exist in DB
                var storedToken = _context.refreshTokens.FirstOrDefault(x => x.Token == tokenModel.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //checked 5: Kieemr tra refreshToken đã được sử dụng hay đá được thu hồi hay chưa
                if (storedToken.IsUsed)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used it"
                    });
                }

                if (storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                // check 6: Kiểm tra Id == JwtId trong refreshToken
                var jti =tokenInveritification.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Jti).Value;
                if(storedToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                // Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                // create new token
                var user = await _context.NguoiDungs.SingleOrDefaultAsync(nd => nd.Id == storedToken.UserId);
                // Cap token
                var token = await GenerateToken(user);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
                });
            

               
            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utxExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utxExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }
    }
}
