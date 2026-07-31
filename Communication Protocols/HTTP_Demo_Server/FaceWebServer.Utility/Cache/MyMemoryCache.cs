using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;

namespace FaceWebServer.Utility
{

    public class MyMemoryCache : IMyMemoryCache
    {
        IMemoryCache Cache;

        public MyMemoryCache(IMemoryCache cache)
        {
            this.Cache = cache;
        }


        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            T value = default(T);
            Cache.TryGetValue<T>(key, out value);

            return value;
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time(unit:minute)</param>
        public void Add<T>(string key, T data, int cacheTime = 30)
        {
            if (data == null)
                return;

            Cache.Set<T>(key, data, TimeSpan.FromMinutes(cacheTime));

        }

        public void Set<T>(string key, T data)
        {
            if (data == null)
                return;
            Cache.Set<T>(key, data);
        }

        /// <summary>
        /// 更新缓存键或重置过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public void Update<T>(string key, T data, int cacheTime = 30)
        {
            if (Contains(key))
            {
                Remove(key);
            }
            Add<T>(key, data, cacheTime);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public bool Contains(string key)
        {
            object value;
            return Cache.TryGetValue(key, out value);
        }


        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            Cache.Remove(key);
        }


        /// <summary>
        /// 根据键值返回缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return Cache.Get(key); }
            set { Add(key, value); }
        }
    }
}
