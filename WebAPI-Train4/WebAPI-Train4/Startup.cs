using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using WebAPI_Train4.Data;
using WebAPI_Train4.Models;
using WebAPI_Train4.Services;

namespace WebAPI_Train4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Define dependence
            services.AddControllers();
            services.AddDbContext<MyDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("MyDB"));
            });

            //services.AddScoped<ICategoryRepository, LoaiRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepositoryInMemory>();
            services.AddScoped<IHangHoaRepository, HangHoaRepository>();
            services.Configure<AppSetting>(Configuration.GetSection("AppSettings"));
           
            // Dùng để mã hóa JWT
            // JWT dùng secretKey để sài
            // Thuật toán mã hóa chỉ sử dụng trên bit cần phải convert về mảng byte
            var secretKey = Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            // AddAuthentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    
                    // Tự cấp token
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    // Ký vào token
                    ValidateIssuerSigningKey = true,
                    // Sử dụng thuật toán đối xứng ứng với cái Key, sẽ tự động mã hóa, về mặt mã hóa thì phải làm được trên bit thì phải encode lại
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI_Train4", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI_Train4 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            // Authentication phải đặt trước Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
