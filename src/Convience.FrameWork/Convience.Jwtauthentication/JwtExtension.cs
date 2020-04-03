using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.Threading.Tasks;

namespace Convience.Jwtauthentication
{
    public static class JwtExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtOption>(configuration)
                .AddScoped<IJwtFactory, JwtFactory>();

            var jwtOption = services.BuildServiceProvider()
                .GetRequiredService<IOptions<JwtOption>>().Value;

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = jwtOption.SecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };

                //option.Events = new JwtBearerEvents
                //{
                //    OnAuthenticationFailed = (context) =>
                //    {
                //        context.Response.StatusCode = 401;
                //        return context.Response.WriteAsync("authentication failed.");
                //    }
                //};
            });

            return services;
        }
    }
}
