using System;
using System.Collections;
using System.ComponentModel;
using EnergyTrading.Conversion;

namespace EnergyTrading.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> MergeLeft<TKey, TValue>(this IDictionary<TKey, TValue> primary, bool overwriteDuplicates, params IDictionary<TKey, TValue>[] secondaries)
        {
            return secondaries.Aggregate(primary, (current, dict) => current.Merge(dict, overwriteDuplicates));
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> primary, IDictionary<TKey, TValue> secondary, bool overwriteDuplicates = false)
        {
            if (primary == null)
            {
                return null;
            }

            if (secondary == null)
            {
                return primary;
            }

            foreach (var kvp in secondary)
            {
                if (!primary.ContainsKey(kvp.Key))
                {
                    primary.Add(kvp);
                }
                else if (overwriteDuplicates)
                {
                    primary[kvp.Key] = kvp.Value;
                }
            }

            return primary;
        }

        private static void AddPropertyToDictionary(this IDictionary<string, string> dictionary, PropertyDescriptor descriptor, object value, bool pathingKeys, string prefix)
        {
            if (descriptor.PropertyType.IsBasicType())
            {
                if (!dictionary.ContainsKey(prefix + descriptor.Name))
                {
                    dictionary.Add(prefix + descriptor.Name, value?.ToString());
                }
            }
            else if (value != null)
            {
                if (descriptor.PropertyType.Implements<IConvertTo<KeyValuePair<string, String>>>())
                {
                    var kvp = ((IConvertTo<KeyValuePair<string, string>>)value).GetPair(prefix);
                    if (!dictionary.ContainsKey(kvp.Key))
                    {
                        dictionary.Add(kvp);
                    }
                }
                else if (descriptor.PropertyType.Implements<IEnumerable>())
                {
                    foreach (var item in ((IEnumerable)value))
                    {
                        dictionary.Merge(item.ToStringDictionary(pathingKeys, prefix + descriptor.Name));
                    }
                }
                else
                {
                    dictionary.Merge(value.ToStringDictionary(pathingKeys, prefix + descriptor.Name));
                }
            }
        }

        private static KeyValuePair<string, string> GetPair(this IConvertTo<KeyValuePair<string, string>> converter, string prefix)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }
            var kvp = converter.Convert();
            if (!string.IsNullOrEmpty(prefix))
            {
                kvp = new KeyValuePair<string, string>(prefix + kvp.Key, kvp.Value);
            }
            return kvp;
        }

        private static IDictionary<string, string> ToStringDictionary(this object source, bool pathingKeys, string currentPath)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            if (source == null || source.IsBasicType())
            {
                return dictionary;
            }
            var sourceType = source.GetType();
            var prefix = pathingKeys && currentPath.Length > 0 ? currentPath + "." : "";
            if (sourceType.Implements<IConvertTo<KeyValuePair<string, String>>>())
            {
                var kvp = ((IConvertTo<KeyValuePair<string, string>>)source).GetPair(prefix);
                if (!dictionary.ContainsKey(kvp.Key))
                {
                    dictionary.Add(kvp);
                }
                return dictionary;
            }

            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(source))
            {
                var value = propertyDescriptor.GetValue(source);
                dictionary.AddPropertyToDictionary(propertyDescriptor, value, pathingKeys, prefix);
            }
            return dictionary;
        }

        public static IDictionary<string, string> ToStringDictionary(this object source, bool pathingKeys = true)
        {
            return source.ToStringDictionary(pathingKeys, "");
        }
    }
}