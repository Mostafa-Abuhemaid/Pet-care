
//using FluentValidation;
//using Mapster;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
//using System.Reflection;
//using Web.Application.DTOs.EmailDTO;
//using Web.Application.Interfaces;
//using Web.Application.Mapping;
//using Web.Domain.Entites;
//using Web.Infrastructure.Data;
//using Web.Infrastructure.Service;

//namespace Web.APIs
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);
//            var configuration = builder.Configuration;

//            builder.Services.AddCors(options =>
//            {
//                options.AddDefaultPolicy(builder =>
//                {
//                    builder
//                        .AllowAnyOrigin()   
//                    //.WithOrigins("https://")
//                        .AllowAnyMethod()
//                        .AllowAnyHeader();
//                });
//            });


//            // Add services to the container.

//            builder.Services.AddControllers();

//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();
//            builder.Services.AddDbContext<AppDbContext>(options =>
//              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
//            builder.Services.AddIdentity<AppUser, IdentityRole>()
//             .AddRoles<IdentityRole>()
//             .AddEntityFrameworkStores<AppDbContext>()

//             .AddDefaultTokenProviders();
//            builder.Services.Configure<EmailDto>(configuration.GetSection("MailSettings"));
//            builder.Services.AddTransient<IEmailService, EmailService>();
//            builder.Services.AddScoped<IAccountService, AccountService>();
//            builder.Services.AddScoped<ITokenService, TokenService>();
//            builder.Services.AddScoped<IUserService, UserService>();
//            builder.Services.AddScoped<IPetProfileService, PetProfileService>();
//            builder.Services.AddAutoMapper(typeof(MappingProfile));
//            builder.Services.AddMemoryCache();
//            builder.Services
//                .AddFluentValidationAutoValidation()
//                .AddValidatorsFromAssembly(Assembly.Load("Web.Application"));
//            builder.Services.AddMapster();


//            var app = builder.Build();

//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//           }
//            app.UseCors();

//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}

using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Text;
using Web.Application.DTOs.EmailDTO;
using Web.Application.Interfaces;
using Web.Application.Mapping;
using Web.Domain.Entites;
using Web.Infrastructure.Data;
using Web.Infrastructure.Service;

namespace Web.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database Configuration
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Identity Configuration
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // JWT Authentication Configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT Key is missing")))
                };
            });

            // Authorization
            builder.Services.AddAuthorization();

            // Email Configuration
            builder.Services.Configure<EmailDto>(configuration.GetSection("MailSettings"));

            // Dependency Injection
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPetProfileService, PetProfileService>();

            // Mapping Configuration
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddMapster();

            // Cache and Validation
            builder.Services.AddMemoryCache();
            builder.Services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.Load("Web.Application"));

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware Pipeline (الترتيب مهم جداً!)
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();  // يجب أن يكون قبل UseAuthorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}