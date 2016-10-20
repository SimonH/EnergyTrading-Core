using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace EnergyTrading.Mapping
{
    public class PerThreadXmlMappingEngine : XmlMappingEngine
    {
        /// <summary>
        /// Create a new instance of the <see cref="XmlMappingEngine" /> class.
        /// </summary>
        /// <param name="factory">Factory to use.s</param>
        public PerThreadXmlMappingEngine(IXmlMapperFactory factory) : base(factory)
        {
        }

        protected override IDictionary<Tuple<string, string>, Type> CreateXmlTypeDictionary()
        {
            return new Dictionary<Tuple<string, string>, Type>();
        }

        protected override void InitializeManagers()
        {
            XmlNamespaceManager = new XmlNamespaceManager(new NameTable());
            NamespaceManager = new PerThreadBaseNamespaceManager(XmlNamespaceManager);
        }
    }
}