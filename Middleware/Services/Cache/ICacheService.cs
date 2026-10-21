
using System;
using System.Threading.Tasks;

namespace Middleware.Services.Cache
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);

        Task SetAsync<T>(string key,T value,TimeSpan? ttl = null);

        Task RemoveAsync(string key);
    }
}
