
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.Mail
{
    public static class MailExtension
    {
        public static IServiceCollection AddMail(this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = "main")
        {
            services.Configure<MailOption>(configuration.GetSection(sectionName));
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }

        public static IServiceCollection AddMail(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MailOption>(configuration);
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }
    }
}
