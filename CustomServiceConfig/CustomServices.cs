using update.Database;
using update.Repositories.Repos;
using update.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using update.Models.Domain;
using Microsoft.AspNetCore.Identity;
using update.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using update.Mappings;

namespace update.CustomServiceConfig;

public static class CustomService
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration _configuration) {
        // swagger configuration        
        services.AddSwaggerGen(config => {
            var jwtScheme = new OpenApiSecurityScheme 
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Jwt Token",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            config.AddSecurityDefinition(jwtScheme.Reference.Id, jwtScheme);

            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    jwtScheme, Array.Empty<string>()
                }
            });
        });

        // DbContext
        services.AddDbContext<ApplicationDbContext>();

        // repositories for dependency injection
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IJobTagRepository, JobTagRepository>();

        // security configuration
        services.AddIdentity<User, IdentityRole>(options => {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        // services for dependency injection
        services.AddScoped<TokenService>();

        // authentication mechanism
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                authenticationScheme: JwtBearerDefaults.AuthenticationScheme,
                options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])
                        )
                    };
            });
        
        // automapper
        services.AddAutoMapper(typeof(CustomMappingProfile));

        return services;
    }
}