using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

namespace Convience.WPFClient.Utils
{
    public static class HttpClientUtil
    {
        private readonly static HttpClient _httpClient;

        static HttpClientUtil()
        {
            if (_httpClient == null)
            {
                var apiUri = ConfigurationUtil.GetApiUri();
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(apiUri);
            }
        }

        public static void AddBearaHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public static void RemoveBearaHeader()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }

        #region 请求

        /// <summary>
        /// get请求
        /// </summary>
        public static async Task<string> GetResponseAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// get请求(泛型)
        /// </summary>
        public static async Task<T> GetResponseAsync<T>(string url) where T : class
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// post
        /// </summary>
        public static async Task<string> PostRequestAsync(string url, string data)
        {
            try
            {
                var httpContent = new StringContent(data);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }
                else
                {
                    await HandleErrorResponse(response);
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// post(泛型)
        /// </summary>
        public static async Task<string> PostRequestAsync<T>(string url, T data) where T : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                var httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }
                else
                {
                    await HandleErrorResponse(response);
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// post(泛型)(有返回类型)
        /// </summary>
        public static async Task<T2> PostRequestAsync<T1, T2>(string url, T1 data)
            where T1 : class where T2 : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                var httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T2>(result);
                }
                else
                {
                    await HandleErrorResponse(response);
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        private static async Task HandleErrorResponse(HttpResponseMessage response)
        {

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    string result = await response.Content.ReadAsStringAsync();
                    var jo = JObject.Parse(result);
                    MessageBox.Show(jo[""].First.ToString());
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
