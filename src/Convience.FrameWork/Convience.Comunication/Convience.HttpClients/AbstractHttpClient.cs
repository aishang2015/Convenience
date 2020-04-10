using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Convience.HttpClients
{
    public abstract class AbstractHttpClient
    {
        private readonly HttpClient _httpClient;

        // 实现类必须有相同的构造函数
        public AbstractHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void ConfigHttpClient(string baseUri, Dictionary<string, string> _headers = null)
        {
            _httpClient.BaseAddress = new Uri(baseUri);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            if (_headers != null)
            {
                _headers.Keys.ToList().ForEach(k =>
                {
                    _httpClient.DefaultRequestHeaders.Add(k, _headers.GetValueOrDefault(k));
                });
            }
        }

        public async Task<string> GetAsync(string uri)
        {
            return await _httpClient.GetStringAsync($"{_httpClient}/uri");
        }

        public async Task<HttpResponseMessage> PostAsync(string uri, object content)
        {
            var hc = CreateStringContent(content);
            return await _httpClient.PostAsync($"{_httpClient}/uri", hc);
        }

        private StringContent CreateStringContent(object body)
        {
            return new StringContent(JsonConvert.SerializeObject(body));
        }
    }
}
