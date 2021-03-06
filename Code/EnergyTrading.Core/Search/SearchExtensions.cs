﻿namespace EnergyTrading.Search
{
    using System;
    using System.Globalization;
    using System.Text;

    using EnergyTrading.Xml.Serialization;

    public static class SearchExtensions
    {
        public static Contracts.Search.Search ToSearch<TContract>(this string key)
        {
            var searchString = FromBase64(key.Replace("-", "/").Replace(")", "+").Replace("(", "=")).Substring(typeof(TContract).Name.Length + 4);
            return searchString.DeserializeDataContractXmlString<Contracts.Search.Search>();
        }

        private static string FromBase64(string source)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(source));
        }

        private static string ToBase64(string source)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }

        public static string ToKey<TContract>(this Contracts.Search.Search search, uint version = 0)
        {
            var searchString = typeof(TContract).Name + version.ToString(CultureInfo.InvariantCulture).PadLeft(4) + search.DataContractSerialize();
            var baseString = ToBase64(searchString);
            return baseString.Replace("/", "-").Replace("+", ")").Replace("=", "(");
        }
    }
}