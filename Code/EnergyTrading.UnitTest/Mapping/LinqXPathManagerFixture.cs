﻿namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using NUnit.Framework;

    /// <summary>
    /// Difference between this and the XPathProcessor is that the qualified values
    /// are suitable for LINQ to XML queries - hence different expected values, e.g.
    /// namespaces rather than prefixes and no qualification for attributes or array values.
    /// </summary>
    [TestFixture]
    public class LinqXPathManagerFixture
    {
        protected INamespaceManager NamespaceManager { get; set; }

        [SetUp]
        public void Initialize()
        {
            this.NamespaceManager = this.CreateNamespaceManager();
            this.NamespaceManager.RegisterNamespace("a", "http://www.a.com");
            this.NamespaceManager.RegisterNamespace("b", "http://www.b.com");
        }

        [Test]
        public void EmptyPrefixNamespace()
        {
            var manager = this.CreateXPathManager();

            var expected = "fred";
            var candidate = manager.QualifyXPath("fred", null);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void PrefixQualified()
        {
            var manager = this.CreateXPathManager();

            var expected = "{http://www.a.com}fred";
            var candidate = manager.QualifyXPath("fred", "a");

            Assert.AreEqual(expected, candidate);   
        }

        [Test]
        public void PrefixPreferenceQualified()
        {
            var manager = this.CreateXPathManager();

            var expected = "{http://www.a.com}fred";
            var candidate = manager.QualifyXPath("fred", "a", "http://www.b.com");

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void NamespaceQualified()
        {
            var manager = this.CreateXPathManager();

            var expected = "{http://www.a.com}fred";
            var candidate = manager.QualifyXPath("fred", null, "http://www.a.com");

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void AttributeXPath()
        {
            var manager = this.CreateXPathManager();

            var expected = "{http://www.a.com}fred";
            var candidate = manager.QualifyXPath("fred", "a", null, -1, true);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void IndexedXPath()
        {
            var manager = this.CreateXPathManager();

            var expected = "{http://www.a.com}fred";
            var candidate = manager.QualifyXPath("fred", "a", null, 0);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void UnregisteredPrefix()
        {
            var manager = this.CreateXPathManager();

            Assert.Throws<MappingException>(() => manager.QualifyXPath("fred", "c")); 
        }

        protected virtual IXPathManager CreateXPathManager()
        {            
            var xp = new LinqXPathManager(this.NamespaceManager);

            return xp;
        }

        protected virtual INamespaceManager CreateNamespaceManager()
        {
            return new NamespaceManager();
        }
    }
}