using System.Collections.Immutable;
using auth_service.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
/*
using Cubitwelve.Src.Repositories;
using Cubitwelve.Src.Repositories.Interfaces;
using Cubitwelve.Src.Services.Interfaces;
using Cubitwelve.Src.Services;
using Cubitwelve.Src.Exceptions;
*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using auth_service.Exceptions;
using auth_service.Repositories.Interfaces;
using auth_service.Services;
using auth_service.Services.Interfaces;
using Microsoft.OpenApi.Models;

namespace auth_service.Extensions;

public static class AppServiceExtensions
{
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            InitEnvironmentVariables();
            AddAutoMapper(services);
            AddServices(services);
            AddSwaggerGen(services);
            AddDbContext(services);
            AddUnitOfWork(services);
            // AddAuthentication(services, config);
            AddHttpContextAccesor(services);
        }

        private static void InitEnvironmentVariables()
        {
            Env.Load();
        }

        // services here...
        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IMapperService, MapperService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        
        // swagger configuration
        private static void AddSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Cubitwelve API", Version = "v1" })
            );

        }

        private static void AddDbContext(IServiceCollection services)
        {
            var connectionUrl = Environment.GetEnvironmentVariable("DB_CONNECTION");
            // var connectionUrl = "Server=localhost;Database=dbo;User Id=sa;Password=yourStrongPassword#1234;TrustServerCertificate=true";
            
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(connectionUrl, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: System.TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                    );
                });
            });
        }

        
        // unit of work here
        private static void AddUnitOfWork(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        // Automappers here
        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }

        
        private static IServiceCollection AddAuthentication(IServiceCollection services, IConfiguration config)
        {
            var jwtSecret = Env.GetString("JWT_SECRET") ??
                throw new InvalidJwtException("JWT_SECRET not present in .ENV");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return services;
        }
        

        private static void AddHttpContextAccesor(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }
        
    
}