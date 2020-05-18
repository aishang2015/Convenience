using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net.Http;

namespace Convience.HttpClients
{
    public static class HttpClientServiceExtension
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddGitHubHttpClient(this IServiceCollection services, string name = "github")
        {
            var fallbackResponse = new HttpResponseMessage();
            fallbackResponse.Content = new StringContent("fallback");
            fallbackResponse.StatusCode = System.Net.HttpStatusCode.TooManyRequests;

            services.AddHttpClient(name, c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/");
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            })
            // 等待，重试
            .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(2, _ => TimeSpan.FromMilliseconds(600)))

            // 降级,返回fallback
            .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().FallbackAsync(fallbackResponse))

            // 熔断器 2次后触发熔断4秒后允许重试
            .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().CircuitBreakerAsync(2, TimeSpan.FromSeconds(4),
                    (ex, ts) => Console.WriteLine($"break here {ts.TotalMilliseconds}"),
                    () => Console.WriteLine($"reset here ")))

            // 设置超时
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
            ;
            return services;
        }
    }
}
