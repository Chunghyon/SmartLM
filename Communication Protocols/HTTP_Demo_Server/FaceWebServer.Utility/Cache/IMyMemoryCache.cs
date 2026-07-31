using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility
{
    public interface IMyMemoryCache
    {
        /// <summary>
        /// 获取缓存的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public T Get<T>(string key);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public void Add<T>(string key, T data, int cacheTime = 30);


        /// <summary>
        /// 添加缓存,永久
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public void Set<T>(string key, T data);


        /// <summary>
        /// 更新缓存键或重置过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public void Update<T>(string key, T data, int cacheTime = 30);

        /// <summary>
        /// 检查缓存键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key);
        /// <summary>
        /// 删除指定的缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key);

        /// <summary>
        /// 获取或添加缓存（添加缓存，默认缓存30分钟）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key] { get; set; }

    }

}
