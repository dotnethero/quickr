using Newtonsoft.Json;
using StackExchange.Redis;

namespace Quickr.Utils
{
    internal static class StringExtensions
    {
        internal static int ToInt32(this string s)
        {
            return int.Parse(s);
        }

        internal static string PrettifyJson(this RedisValue json)
        {
            if (!json.HasValue) return json;
            if (!json.StartsWith("{") && !json.StartsWith("[")) return json;
            try
            {
                var obj = JsonConvert.DeserializeObject(json);
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch (JsonReaderException)
            {
                return json;
            }
        }
    }
}