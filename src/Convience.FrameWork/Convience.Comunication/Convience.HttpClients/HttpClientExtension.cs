using Newtonsoft.Json;

using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Convience.HttpClients
{
    public static class HttpClientExtension
    {
        public static async Task<T> Get<T>(this HttpResponseMessage httpResponseMessage)
        {
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static void Handle(this HttpResponseMessage httpResponseMessage,
            [Optional]Action<HttpResponseMessage> successAction,
            [Optional]Action<HttpResponseMessage> failAction)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                successAction(httpResponseMessage);
            }
            else
            {
                failAction(httpResponseMessage);
            }
        }
    }
}
