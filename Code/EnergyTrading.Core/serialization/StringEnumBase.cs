using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnergyTrading.serialization
{
    public abstract class StringEnumBase<T> where T : class 
    {
        protected StringEnumBase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            Value = value;
        }

        private static IEnumerable<T> GetValidValues()
        {
            var infos = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);
            return infos.Select(info => info.GetValue(null)).Cast<T>();
        }

        private static bool AreEqual(string source, T item, bool matchCase)
        {
            if (item == null)
            {
                return source == null;
            }

            return matchCase ? item.ToString() == source : string.Compare(source, item.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public static bool TryFromString(string status, out T result, bool matchCase = false)
        {
            result = default(T);
            try
            {
                result = FromString(status, matchCase, AreEqual);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static T FromString(string status, bool matchCase = false)
        {
            return FromString(status, matchCase, AreEqual);
        }

        private static T FromString(string source, bool matchCase, Func<string, T, bool, bool> equalityFunc)
        {
            if (equalityFunc == null)
            {
                throw new ArgumentNullException(nameof(equalityFunc));
            }

            foreach (var value in GetValidValues().Where(val => equalityFunc(source, val, matchCase)))
            {
                return value;
            }
            throw new ArgumentOutOfRangeException(nameof(source), source + " is not a valid " + typeof(T).Name);
        }

        protected string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as T;
            if (other != null)
            {
                return Value.Equals(other.ToString());
            }
            return false;
        }

        public bool Equals(string str)
        {
            return !string.IsNullOrWhiteSpace(str) && str == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}