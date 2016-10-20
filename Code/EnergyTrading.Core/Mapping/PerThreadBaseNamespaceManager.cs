using System;
using System.Xml;
using EnergyTrading.Extensions;

namespace EnergyTrading.Mapping
{
    public class PerThreadBaseNamespaceManager : BaseNamespaceManager
    {
        public PerThreadBaseNamespaceManager(XmlNamespaceManager manager) : base(manager)
        {
        }

        protected override Func<string, string> InitialiseNamespaceQualifier()
        {
            Func<string, string> f = QualifyNamespace;
            return f.SingleThreadMemoize();
        }
    }
}