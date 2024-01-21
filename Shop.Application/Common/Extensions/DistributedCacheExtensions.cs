using Shop.Application.Cart.Models;
using Shop.Domain.Constraints;
using System.Text.Json;

namespace Microsoft.Extensions.Caching.Distributed;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache distributedCache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null) where T : class
    {
        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
        options.SlidingExpiration = unusedExpireTime;

        var jsonData = JsonSerializer.Serialize(data);
        await distributedCache.SetStringAsync(recordId, jsonData, options);
    }

    public static async Task<T?> GetRecordAsync<T>(this IDistributedCache distributedCache, string recordId) where T : class
    {
        var jsonData = await distributedCache.GetStringAsync(recordId);
        if (jsonData == null)
        {
            return null;
        }

        await distributedCache.RefreshAsync(recordId);
        var record = JsonSerializer.Deserialize<T>(jsonData);

        return record;
    }

    public static async Task SetCartAsync(this IDistributedCache distributedCache, CartDto cart)
    {
        await distributedCache.SetRecordAsync(cart.CartId.ToString(), cart, CartConstrains.Expiration.AbsoluteExpireTime, CartConstrains.Expiration.UnusedExpireTime);
    }

    public static async Task<CartDto?> GetCartAsync(this IDistributedCache distributedCache, Guid cartId)
    {
        return await distributedCache.GetRecordAsync<CartDto>(cartId.ToString());
    }
}
