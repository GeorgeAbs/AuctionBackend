using Microsoft.Extensions.Caching.Distributed;

namespace Application.Extensions
{
    public static class CacheExtension
    {
        public async static Task<string?> GetStringByCompKeyAsync(this IDistributedCache cache, Type objectType, string key)
        {
            return await cache.GetStringAsync(CreateKey(objectType, key));
        }

        public async static Task SetStringByCompKeyAsync(this IDistributedCache cache, Type objectType, string key, string value)
        {
            await cache.SetStringAsync(CreateKey(objectType, key), value);
            return;
        }

        public async static Task RemoveStringByCompKeyAsync(this IDistributedCache cache, Type objectType, string key)
        {
            await cache.RemoveAsync(CreateKey(objectType, key));
            return;
        }

        /*public async static Task RefreshStringByCompKeyAsync(this IDistributedCache cache, Type objectType, string key, string value)
        {
            await cache.RefreshAsync(CreateKey(objectType, key));
            return;
        }*/

        private static string CreateKey(Type objectType, string key)
        {
            return $"{objectType.Name}_{key}";
        }
    }
}
