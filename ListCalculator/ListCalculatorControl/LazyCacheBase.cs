using System;
using System.Collections.Generic;
using System.Linq;

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
        protected void InvalidateValues(Func<TKey, bool> selector) {
            List<TKey> keysToRemove = Cache.Keys.Where(selector).ToList();
            foreach(TKey key in keysToRemove)
                Cache.Remove(key);
        }
        protected abstract TValue GetValueFor(TKey key);
    }
}
