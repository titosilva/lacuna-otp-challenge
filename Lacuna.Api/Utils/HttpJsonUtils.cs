using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Lacuna.Api.Utils
{
    public static class HttpJsonUtils
    {
        public static HttpContent GenerateContent(object data)
        {
            string json = JsonSerializer.Serialize(data);

            var content = new StringContent(json.ToString(), Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }

        public static Dictionary<string, string>  DeserializeToDict(string json)
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
    }
}