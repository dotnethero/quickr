using StackExchange.Redis;

namespace Quickr.Utils
{
    internal static class RedisExtensions
    {
        internal static IServer GetServer(this IConnectionMultiplexer mp)
        {
            return mp.GetServer("localhost", 6379);
        }
    }
}