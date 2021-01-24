using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Polly;
using Polly.Extensions.Http;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Convience.Util.Extension
{
    public static class HttpClientExtension
    {
        /// <summary>
        /// 将对象转化为httpstringcontent
        /// </summary>
        public static StringContent ToStringContent(this object o)
        {
            var content = new StringContent(JsonConvert.SerializeObject(o));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        /// <summary>
        /// 读取结果转化为对象
        /// </summary>
        public static async Task<JObject> GetResult(this HttpResponseMessage httpResponse)
        {
            var body = await httpResponse.Content.ReadAsStringAsync();
            return JObject.Parse(body);
        }

        /// <summary>
        /// 使用polly策略的httpclient
        /// </summary>
        public static IServiceCollection AddPollyClient(this IServiceCollection services,
            string clientName,
            string baseAddress,
            IAsyncPolicy<HttpResponseMessage> retryPolicy = null,
            IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy = null,
            IAsyncPolicy<HttpResponseMessage> timeoutPolicy = null)
        {
            services.AddHttpClient(clientName, c =>
            {
                c.BaseAddress = new Uri(baseAddress);
                c.DefaultRequestHeaders.Add("User-Agent", "CSharp-HttpClient");
            })
            .AddPolicyHandler(retryPolicy ?? GetRetryPolicy())
            .AddPolicyHandler(circuitBreakerPolicy ?? GetCircuitBreakerPolicy())
            .AddPolicyHandler(timeoutPolicy ?? GetTimeoutPolicy());
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
