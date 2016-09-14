using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace EnergyTrading.Configuration
{
    public class NamedItemsSection : ConfigurationSection
    {
        [ConfigurationProperty("items", IsDefaultCollection = true)]
        public NamedItemsCollection Items
        {
            get { return (NamedItemsCollection) this["items"]; }
            set { this["items"] = value; }
        }

        public List<string> ToStringList()
        {
            var list = new List<string>();
            if (Items != null && Items.Count > 0)
            {
                list.AddRange(Items.Select(i => i.Name));
            }
            return list;
        }
    }
}