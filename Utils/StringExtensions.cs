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
            dynamic obj = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}