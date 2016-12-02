using System.Collections.Generic;
using System.Dynamic;
using EnergyTrading.Extensions;

namespace EnergyTrading.Types
{
    public static class AnonymousTypeParser
    {
        public static IDictionary<string, object> ToDictionary(this object instance)
        {
            var ret = new Dictionary<string, object>();
            if (instance != null)
            {
                foreach (var prop in instance.GetType().GetProperties())
                {
                    ret.Add(prop.Name, prop.GetValue(instance));
                }
            }
            return ret;
        }

        public static dynamic ToDynamic(this object instance)
        {
            var instanceDict = instance.ToDictionary();
            var ret = new ExpandoObject();
            var retDict = ret as IDictionary<string, object>;
            foreach (var pair in instanceDict)
            {
                retDict.Add(pair.Key, pair.Value);
            }
            return ret;
        }
    }
}