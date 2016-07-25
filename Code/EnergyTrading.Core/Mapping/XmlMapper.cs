﻿namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using EnergyTrading.Xml.Linq;

    /// <summary>
    /// Base implementation of a two-way <see cref="IXmlMapper{T, XElement}" /> for an entity using XPath.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class XmlMapper<TEntity> : XPathMapper<TEntity>, IXmlMapper<TEntity, XElement>
        where TEntity : class, new()
    {
        protected const string CurrentXPath = "";

        protected XmlMapper() : this(string.Empty)
        {
        }

        protected XmlMapper(string nodeName) : this(nodeName, null)
        {
        }

        protected XmlMapper(IXmlMappingEngine engine) : this(string.Empty, engine)
        {
        }

        protected XmlMapper(string nodeName, IXmlMappingEngine engine) : base(nodeName, engine)
        {
            this.AttributeDefaultNamespace = true;
        }

        /// <summary>
        /// Gets or sets whether attributes use the default namespace.
        /// </summary>
        protected bool AttributeDefaultNamespace { get; set; }

        /// <copydocfrom cref="IMapper{TEntity, XElement}.Map(TEntity)" />
        public XElement Map(TEntity source)
        {
            return this.Map(source, this.NodeName);
        }

        /// <copydocfrom cref="IXmlMapper{T, XElement}.Map(T, string, string, bool)" />
        public XElement Map(TEntity source, string nodeName, string xmlNamespace = null, bool outputDefault = false)
        {
            if (source == null && !outputDefault)
            {
                return null;
            }

            // Ensure our namespace prefix is available
            this.Engine.RegisterNamespace(this.NamespacePrefix, this.Namespace);

            // And we have a node name
            nodeName = !string.IsNullOrEmpty(nodeName) ? nodeName : this.NodeName;

            // Ok create the destination
            var destination = this.CreateElement(nodeName, xmlNamespace);

            // Needed for any possible default values in the entity
            if (source == null)
            {
                source = this.CreateDestination();
            }

            this.Map(source, destination);

            // Only emit the type if we have content
            if (!string.IsNullOrEmpty(this.XmlType) && destination.HasElements)
            {
                this.EmitXmlType(destination);
            }

            return destination;
        }

        public abstract void Map(TEntity source, XElement destination);

        public List<XElement> MapList(TEntity source, string collectionNode, bool outputDefault = false)
        {
            return this.MapList(source, collectionNode, this.NodeName, outputDefault);
        }

        public List<XElement> MapList(TEntity source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            var list = new List<XElement>();

            return list;
        }

        public List<XElement> MapList(TEntity source, string collectionNode, string nodeName, string collectionNodeNamespacePrefix = "", string collectionItemNodeNamespacePrefix = "", bool outputDefault = false)
        {
            return new List<XElement>();
        }

        public virtual XElement MapList(IEnumerable<TEntity> source, string collectionNode, bool outputDefault = false)
        {
            return this.MapList(source, collectionNode, this.NodeName, outputDefault);
        }

        public virtual XElement MapList(IEnumerable<TEntity> source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            return this.MapList(source, collectionNode, nodeName, this.Namespace, this.Namespace, outputDefault);
        }

        public virtual XElement MapList(IEnumerable<TEntity> source, string collectionNode, string collectionItemNodeName, string collectionNodeNamespace = "", string collectionItemNodeNamespace = "", bool outputDefault = false)
        {
            var values = source == null ? new List<TEntity>() : source.ToList();
            if (values.Count == 0 && outputDefault == false)
            {
                return null;
            }

            return new XElement(this.QualifiedName(collectionNode, collectionNodeNamespace), this.ElementList(values, collectionItemNodeName, collectionItemNodeNamespace));
        }

        /// <summary>
        /// Determine if the entity has any significant content, used for optional output control.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>true if we have significant content, false otherwise.</returns>
        /// <remarks>
        /// Significant content typically means that we have a non-empty node somewhere in the entity
        /// or its children so that it's worthwhile performing the serialization.
        /// </remarks>
        protected virtual bool HasSignificantContent(TEntity entity)
        {
            return entity != null;
        }

        /// <summary>
        /// Emit a <see cref="System.Xml.Linq.XElement" /> if it has contents.
        /// </summary>
        /// <param name="name">Name to use</param>
        /// <param name="elements">Elements to use</param>
        /// <param name="xmlNamespace">Namespace to use</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement OptionalXElement(string name, XElement[] elements, string xmlNamespace = "", bool outputDefault = false)
        {
            return OptionalXElement(name, elements.ToList(), xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Emit a <see cref="System.Xml.Linq.XElement" /> if it has contents.
        /// </summary>
        /// <param name="name">Name to use</param>
        /// <param name="elements">Elements to use</param>
        /// <param name="xmlNamespace">Namespace to use</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement OptionalXElement(string name, IList<XElement> elements, string xmlNamespace = "", bool outputDefault = false)
        {
            var values = elements.Where(v => v != null).ToList();
            var content = values.Count == 0 ? null : values;

            return this.XElement(name, content, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        protected XAttribute XAttribute(string name, bool value, string xmlNamespace = "", bool outputDefault = false, bool defaultValue = false)
        {
            return value.ToXAttribute(name, this.CanonicalAttributeNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        protected XAttribute XAttribute(string name, string value, string xmlNamespace = "", bool outputDefault = false, string defaultValue = null)
        {
            return value.ToXAttribute(name, this.CanonicalAttributeNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        protected XAttribute XAttribute(string name, int value, string xmlNamespace = "", bool outputDefault = false, int defaultValue = int.MinValue)
        {
            return value.ToXAttribute(name, this.CanonicalAttributeNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        protected XAttribute XAttribute(string name, decimal value, string xmlNamespace = "", bool outputDefault = false, decimal defaultValue = decimal.MinValue)
        {
            return value.ToXAttribute(name, this.CanonicalAttributeNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="format">Format to use, if null uses the invariant culture DateTimeFormatInfo</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        protected XAttribute XAttribute(string name, DateTime value, string xmlNamespace = "", bool outputDefault = false, string format = null)
        {
            return value.ToXAttribute(name, this.CanonicalAttributeNamespace(xmlNamespace), outputDefault, format);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="format">Format to use, if null uses the invariant culture DateTimeFormatInfo</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        protected XAttribute XAttribute(string name, DateTimeOffset value, string xmlNamespace = "", bool outputDefault = false, string format = null)
        {
            return value.ToXAttribute(name, this.CanonicalAttributeNamespace(xmlNamespace), outputDefault, format);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        protected XAttribute XAttribute(string name, Enum value, string xmlNamespace = "", bool outputDefault = false)
        {
            return value.ToXAttribute(name, this.CanonicalAttributeNamespace(xmlNamespace), outputDefault);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        protected XAttribute XAttribute(string name, object value, string xmlNamespace = "", bool outputDefault = false, object defaultValue = null)
        {
            return value.ToXAttribute(name, this.CanonicalAttributeNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>        
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement XElement(string name, bool value, string xmlNamespace = "", bool outputDefault = false, bool defaultValue = false)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>        
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement XElement(string name, string value, string xmlNamespace = "", bool outputDefault = false, string defaultValue = null)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>        
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement XElement(string name, int value, string xmlNamespace = "", bool outputDefault = false, int defaultValue = int.MinValue)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>        
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement XElement(string name, decimal value, string xmlNamespace = "", bool outputDefault = false, decimal defaultValue = decimal.MinValue)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="format">Format to use, if null uses the invariant culture DateTimeFormatInfo</param>        
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement XElement(string name, DateTime value, string xmlNamespace = "", bool outputDefault = false, string format = null)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault, format);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="format">Format to use, if null uses the invariant culture DateTimeFormatInfo</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement XElement(string name, DateTimeOffset value, string xmlNamespace = "", bool outputDefault = false, string format = null)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault, format);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement XElement(string name, Enum value, string xmlNamespace = "", bool outputDefault = false)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">The default value to compare with.</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        protected XElement XElement(string name, object value = null, string xmlNamespace = "", bool outputDefault = false, object defaultValue = null)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault, defaultValue);
        }

        /// <summary>
        /// Construct an XAttribute for the XML type declaration.
        /// </summary>
        /// <param name="xmlType"></param>
        /// <param name="xmlTypeNamespace"></param>
        /// <returns></returns>
        protected XAttribute XmiTypeAttribute(string xmlType, string xmlTypeNamespace = "")
        {
            if (string.IsNullOrEmpty(xmlType))
            {
                return null;
            }

            if (!xmlType.Contains(":") && !string.IsNullOrEmpty(xmlTypeNamespace))
            {
                var prefix = this.Engine.LookupPrefix(xmlTypeNamespace);
                xmlType = this.QualifiedPrefixName(xmlType, prefix);
            }

            return this.XAttribute("type", xmlType, XsiNamespace);
        }

        private void EmitXmlType(XElement destination)
        {
            if (!string.IsNullOrEmpty(this.XmlTypeNamespacePrefix))
            {
                // Make sure we have a namespace attribute for our XML types prefix
                destination.Add(new XAttribute(XNamespace.Xmlns + this.XmlTypeNamespacePrefix, this.XmlTypeNamespace));
            }

            destination.Add(new XAttribute(this.QualifiedName("type", XsiNamespace), this.QualifiedPrefixName(this.XmlType, this.XmlTypeNamespacePrefix)));
        }

        /// <summary>
        /// By default attributes are not in the default namespace
        /// </summary>
        /// <param name="xmlNamespace">Namespace to use</param>
        /// <returns>Supplied namespace if <see cref="AttributeDefaultNamespace"/> is true, <see cref="CanonicalNamespace"/> otherwise</returns>
        private string CanonicalAttributeNamespace(string xmlNamespace = "")
        {
            return this.AttributeDefaultNamespace ? xmlNamespace : this.CanonicalNamespace(xmlNamespace);
        }

        /// <summary>
        /// Work out whether to deliver the supplied or default namespace
        /// </summary>
        /// <param name="xmlNamespace"></param>
        /// <returns></returns>
        private string CanonicalNamespace(string xmlNamespace = "")
        {
            return !string.IsNullOrEmpty(xmlNamespace) ? xmlNamespace : this.Namespace;
        }

        private XElement CreateElement(string nodeName, string xmlNamespace = "")
        {
            return new XElement(this.QualifiedName(nodeName, this.CanonicalNamespace(xmlNamespace)));
        }

        private List<XElement> ElementList(IEnumerable<TEntity> source, string nodeName, string nodeNamespace = "")
        {
            var list = new List<XElement>();
            var ns = string.IsNullOrWhiteSpace(nodeNamespace) ? this.Namespace : nodeNamespace;

            foreach (var item in source)
            {
                if (item.GetType() == typeof(TEntity))
                {
                    list.Add(this.Map(item, nodeName, ns));
                }
                else
                {
                    // Use dynamic to get to the correct subtype - then let the engine figure it out
                    dynamic child = item;
                    list.Add(this.Engine.Map(child, nodeName, ns));
                }
            }

            return list;
        }

        private XName QualifiedName(string name, string ns = "")
        {
            if (string.IsNullOrEmpty(ns))
            {
                return name;
            }

            XNamespace xns = ns;
            return xns + name;
        }

        private string QualifiedPrefixName(string name, string prefix = "")
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return name;
            }

            return prefix + ":" + name;
        }
    }
}