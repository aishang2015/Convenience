using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Convience.MediatRs
{
    public static class MediatRExtension
    {
        public static IServiceCollection AddMeidatRs(this IServiceCollection services, params Type[] types)
        {
            services.AddMediatR(types.Select(type => type.Assembly).ToArray());
            return services;
        }
    }
}
