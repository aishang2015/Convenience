﻿using backend.util;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace backend.fluentvalidation
{
    public static class FluentValidationExtension
    {
        public static IMvcBuilder AddFluentValidation(this IMvcBuilder builder, IServiceCollection services)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            builder.AddFluentValidation(configuration =>
            {
                var assemblies = ReflectionUtil.AssemblyList;
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
