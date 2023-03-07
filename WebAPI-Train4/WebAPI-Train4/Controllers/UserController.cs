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

namespace WebAPI_Train4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly AppSetting _appSettings;

        public UserController(MyDbContext myDbContext,IOptionsMonitor<AppSetting> options) 
        {
            _context = myDbContext;
            _appSettings = options.CurrentValue;
        }

        [HttpPost("Login")]
        public IActionResult validate(LoginModel loginModel)
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
            return Ok(new ApiResponse
            {
                Success=true,
                Message="Authenticate success",
                Data=GenerateToken(user)
            });
        }

        // Sinh ma token
        private string GenerateToken(NguoiDung nguoiDung)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyByte = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDecription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim(ClaimTypes.Email, nguoiDung.Email),
                    new Claim("UserName", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.Id.ToString()),

                    //roles
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),//TIme out
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(secretKeyByte),SecurityAlgorithms.HmacSha512Signature)

              
            };

            var token = jwtTokenHandler.CreateToken(tokenDecription);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
