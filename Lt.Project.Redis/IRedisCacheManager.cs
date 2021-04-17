using System;
using System.Collections.Generic;
using System.Text;

namespace Lt.Project.Redis
{
    /// <summary>
    /// Redis缓存接口
    /// </summary>
    public interface IRedisCacheManager
    {
        void Clear();
        bool Get(string key);
        string GetValue(string key);
        TEntity Get<TEntity>(string key);
        void Remove(string key);
        void Set(string key, object value, TimeSpan cacheTime);
        bool SetValue(string key, byte[] value);
    }
}
