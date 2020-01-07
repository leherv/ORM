using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Extensions
{
    public static class DictExtensions
    {
        public static TValue GetOrInsert<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue toInsert)
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }
            dict[key] = toInsert;
            return toInsert;
        }
    }
}
