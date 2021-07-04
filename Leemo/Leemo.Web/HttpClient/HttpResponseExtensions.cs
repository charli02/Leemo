using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace Leemo.Web.HttpClient
{
    public static class HttpResponseExtensions
    {
        public static Dictionary<string, dynamic> ContentAsType<T>(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            //return string.IsNullOrEmpty(data) ?
            //                default(T) :
            //                JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);

            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);
        }

        public static string ContentAsJson(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.SerializeObject(data);
        }

        public static string ContentAsString(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}