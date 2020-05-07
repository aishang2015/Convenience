using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Convience.Jwtauthentication
{
    public static class JwtExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
            string authenticationScheme,
            IConfiguration configuration)
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
