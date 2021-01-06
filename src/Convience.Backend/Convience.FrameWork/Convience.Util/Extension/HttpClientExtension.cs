using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    }
}
