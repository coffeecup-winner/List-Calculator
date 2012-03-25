using System;
using System.Collections.Generic;

namespace ListCalculatorControl {
    public abstract class LazyCacheBase<TKey, TValue>
    where TValue : class {
        readonly Dictionary<TKey, TValue> cache = new Dictionary<TKey, TValue>();

        protected Dictionary<TKey, TValue> Cache { get { return cache; } }
        protected TValue GetCachedOrCacheNew(TKey key) {
            return GetCachedOrNull(key) ?? CacheNewValueFor(key);
        }
        protected TValue GetCachedOrNull(TKey key) {
            TValue result;
            return Cache.TryGetValue(key, out result) ? result : null;
        }
        TValue CacheNewValueFor(TKey key) {
            TValue result = GetValueFor(key);
            Cache.Add(key, result);
            return result;
        }
        protected abstract TValue GetValueFor(TKey key);
    }
}
