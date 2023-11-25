using System.Text.Json;

namespace Microsoft.Extensions.Caching.Distributed;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache distributedCache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
        options.SlidingExpiration = unusedExpireTime;

        var jsonData = JsonSerializer.Serialize(data);
        await distributedCache.SetStringAsync(recordId, jsonData, options);
    }
}
