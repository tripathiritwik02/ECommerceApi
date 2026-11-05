using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace ECommerceApi.Caching;

public class ProductCaching
{
    private readonly IMemoryCache _cache;
    private CancellationTokenSource _resetToekn = new CancellationTokenSource();

    public ProductCaching(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T getorcreate<T>(string key, Func<T> factory)
    {
        if (_cache.TryGetValue(key, out object? cached) && cached is T hit)
        return hit;

        var token = _resetToekn.Token;
        var value = factory();

        if (value is not null)
        {
            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)).AddExpirationToken(new CancellationChangeToken(token));
            _cache.Set(key,value,options);
        }
        return value;
    }
    public void Clear()
    {
        var old = Interlocked.Exchange(ref _resetToekn, new CancellationTokenSource());
        old.Cancel();
    }
}