using Convience.Fluentvalidation;
using Convience.Util.Helpers;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.Fluentvalidation
{
    public static class FluentValidationExtension
    {
        public static IMvcBuilder AddFluentValidation(this IMvcBuilder builder, IServiceCollection services)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            builder.AddFluentValidation(configuration =>
            {
                var assemblies = ReflectionHelper.AssemblyList;
                configuration.RegisterValidatorsFromAssemblies(assemblies);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                // 覆盖ModelState管理的默认行为
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    return new BadRequestObjectResult(context.ModelState);
                };
            });

            return builder;
        }
    }
}
