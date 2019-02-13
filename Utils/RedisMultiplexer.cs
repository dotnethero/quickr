using StackExchange.Redis;

namespace Quickr.Utils
{
    internal static class RedisMultiplexer
    {
        internal static IConnectionMultiplexer Connect()
        {
            var options = new ConfigurationOptions
            {
                AllowAdmin = true,
                EndPoints =
                {
                    "localhost"
                }
            };

            return ConnectionMultiplexer.Connect(options);
        }
    }
}