using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Text;
using Web.Application.DTOs.EmailDTO;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Helpers;
using Web.Application.Interfaces;
using Web.Application.Mapping;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;
using Web.Infrastructure.Seeding;
using Web.Infrastructure.Service;
using Web.Infrastructure.Service.Stripe;

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
            builder.Services.Configure<StripeSettings>(configuration.GetSection("StripePaymentService"));

            // Dependency Injection
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped(typeof(BasePetService<>));
            builder.Services.AddScoped<PetServiceFactory>(); 
            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddScoped<DataSeeder>();
            builder.Services.AddScoped<ApplicationSeeder>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<PricingService>();
            builder.Services.AddScoped<IPromoCodeService, PromoCodeService>();
            builder.Services.AddScoped<IPaymentService, StripePaymentService>();

            // Mapping Configuration ==> AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            //////////////////Mapster//////////
            // قراءة BaseURL من appsettings.json
            var baseUrl = builder.Configuration["BaseURL"];
            //  Mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            // Configurations with BaseURL
            MapsterConfiguration.RegisterMappings(mappingConfig, baseUrl);
            builder.Services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            // Cache and Validation
            builder.Services.AddMemoryCache();
            builder.Services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblies(new[]
                {
        Assembly.GetExecutingAssembly(),           // Current Web.APIs
        typeof(PetRequest).Assembly,               // Web.Application  
        typeof(AppDbContext).Assembly              // Web.Infrastructure
                });


            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PetPaw", Version = "v1" });

                // إضافة دعم Authorization في Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: Bearer eyJhbGciOiJIUzI1..."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });

            //Add cache
            builder.Services.AddDistributedMemoryCache();


            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();

                context.Database.Migrate(); // بدل MigrateAsync
                seeder.SeedAllAsync().Wait(); // بدل await
            }



            // Configure the HTTP request pipeline
            //    if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();  
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}