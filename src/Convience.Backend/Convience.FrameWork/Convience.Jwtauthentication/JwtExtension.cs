using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Convience.JwtAuthentication
{
    public static class JwtExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
            IConfiguration configuration,
            string authenticationScheme = "Bearer",
            JwtBearerEvents events = null)
        {
            authenticationScheme = authenticationScheme ?? JwtAuthenticationSchemeConstants.DefaultAuthenticationScheme;

            services.Configure<JwtOption>(authenticationScheme, configuration);

            var jwtOption = services.BuildServiceProvider()
                .GetRequiredService<IOptionsSnapshot<JwtOption>>().Get(authenticationScheme);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(authenticationScheme, option =>
             {
                 option.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateAudience = false,
                     ValidateIssuer = false,
                     IssuerSigningKey = jwtOption.SecurityKey,
                     ValidateIssuerSigningKey = true,
                     ValidateLifetime = true
                 };

                 if (events != null)
                 {
                     option.Events = events;
                 }
             });

            return services;
        }
    }
}
