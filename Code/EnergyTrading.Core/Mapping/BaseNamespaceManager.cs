namespace EnergyTrading.Mapping
{
    using System;
    using System.Xml;

    using EnergyTrading.Extensions;

    /// <summary>
    /// Base implementation of <see cref="INamespaceManager" /> that uses a <see cref="XmlNamespaceManager"/>.
    /// </summary>
    public class BaseNamespaceManager : INamespaceManager
    {
        private readonly XmlNamespaceManager _manager;

        private Func<string, string> _nsQual;

        protected virtual Func<string, string> InitialiseNamespaceQualifier()
        {
            Func<string, string> f = QualifyNamespace;
            return f.Memoize();
        }

        private Func<string, string> NsQual => _nsQual ?? (_nsQual = InitialiseNamespaceQualifier());

        /// <summary>
        /// Creates a new instance of the <see cref="BaseNamespaceManager" /> class.
        /// </summary>
        /// <param name="manager">XmlNamespaceManager to use.</param>
        public BaseNamespaceManager(XmlNamespaceManager manager)
        {
            _manager = manager;
        }

        /// <contentfrom cref="INamespaceManager.RegisterNamespace" />
        public void RegisterNamespace(string prefix, string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return;
            }

            var pf2 = LookupNamespace(prefix);
            if (string.IsNullOrEmpty(pf2))
            {
                if (prefix.Contains(":"))
                {
                    throw new XmlException("The ':' character, hexadecimal value 0x3A, cannot be included in a name - " + prefix);
                }
                _manager.AddNamespace(prefix, uri);
            }            
        }

        /// <contentfrom cref="INamespaceManager.LookupNamespace" />
        public string LookupNamespace(string prefix, bool xname = false)
        {
            var uri = _manager.LookupNamespace(prefix);
            return uri == string.Empty || !xname ? uri : NsQual(uri);
        }

        /// <contentfrom cref="INamespaceManager.LookupPrefix" />
        public string LookupPrefix(string uri)
        {
            return _manager.LookupPrefix(uri);
        }

        /// <contentfrom cref="INamespaceManager.NamespaceExists" />
        public bool NamespaceExists(string uri)
        {
            return !string.IsNullOrEmpty(_manager.LookupPrefix(uri));
        }

        /// <contentfrom cref="INamespaceManager.PrefixExists" />
        public bool PrefixExists(string prefix)
        {
            return !string.IsNullOrEmpty(_manager.LookupNamespace(prefix));
        }

        protected string QualifyNamespace(string uri)
        {
            return uri == string.Empty ? string.Empty : string.Format("{{{0}}}", uri);
        }
    }
}