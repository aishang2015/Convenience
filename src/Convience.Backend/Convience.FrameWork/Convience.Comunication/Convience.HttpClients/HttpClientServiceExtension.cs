using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.Extensions.Http;

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
            //var fallbackResponse = new HttpResponseMessage();
            //fallbackResponse.Content = new StringContent("fallback");
            //fallbackResponse.StatusCode = System.Net.HttpStatusCode.TooManyRequests;

            services.AddHttpClient(name, c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/");
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy())
            .AddPolicyHandler(GetTimeoutPolicy());
            return services;
        }

        /// <summary>
        /// 重试策略
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()   // 5xx,408情况
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) // 404情况
              .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        /// 熔断策略
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()  // System.Net.Http.HttpRequestException情况
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// 超时策略
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(5);
        }
    }
}
