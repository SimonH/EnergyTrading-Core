using System;
using System.Globalization;
using EnergyTrading.Conversion;

namespace EnergyTrading.UnitTest.Extensions
{
    using System.Collections.Generic;

    using EnergyTrading.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class DictionaryExtensionsFixture
    {
        private class TopLevel
        {
            public int Int { get; set; }
            public string String { get; set; }
            public DateTime DateTime { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public Converter Converter { get; set; }
            public Child Child { get; set; }
            public List<Child> ChildList { get; set; }
        }

        private class Child
        {
            public decimal Decimal { get; set; }
            public double Double { get; set; }
            public Child GrandChild { get; set; }
            public Converter Converter { get; set; }
            public List<Child> GrandChildList { get; set; }
        }

        private class Converter : IConvertTo<KeyValuePair<string, string>>
        {
            private readonly string value;
            public Converter()
                : this("value")
            {
            }

            public Converter(string value)
            {
                this.value = value;
            }

            public KeyValuePair<string, string> Convert()
            {
                return new KeyValuePair<string, string>("key", value);
            }
        }

        [Test]
        public void MergeReturnsNullIfSourceIsNull()
        {
            var candidate = DictionaryExtensions.Merge(null, new Dictionary<string, string> { { "key", "value" } });
            Assert.IsNull(candidate);
        }

        [Test]
        public void MergeReturnsSourceIfDictToMergeIsNull()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(null);
            Assert.IsNotNull(candidate);
            Assert.AreSame(source, candidate);
        }

        [Test]
        public void DictionariesAreMerged()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(new Dictionary<string, string> { { "key2", "value2" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(2, candidate.Count);
            Assert.AreEqual("value", candidate["key"]);
            Assert.AreEqual("value2", candidate["key2"]);
        }

        [Test]
        public void DuplicatesAreNotOverwrittenByDefault()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(new Dictionary<string, string> { { "key", "value2" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(1, candidate.Count);
            Assert.AreEqual("value", candidate["key"]);
        }

        [Test]
        public void DuplicatesOverwriteIfSpecified()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(new Dictionary<string, string> { { "key", "value2" } }, true);
            Assert.IsNotNull(candidate);
            Assert.AreEqual(1, candidate.Count);
            Assert.AreEqual("value2", candidate["key"]);
        }

        [Test]
        public void MergeLeftWithoutOverwrites()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.MergeLeft(false, new Dictionary<string, string> { { "key", "value2" } }, new Dictionary<string, string> { { "newKey", "newValue" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(2, candidate.Count);
            Assert.AreEqual("value", candidate["key"]);
            Assert.AreEqual("newValue", candidate["newKey"]);
        }

        [Test]
        public void MergeLeftWithOverwrites()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.MergeLeft(true, new Dictionary<string, string> { { "key", "value2" } }, new Dictionary<string, string> { { "newKey", "newValue" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(2, candidate.Count);
            Assert.AreEqual("value2", candidate["key"]);
            Assert.AreEqual("newValue", candidate["newKey"]);
        }

        [Test]
        public void NullObjectReturnsEmptyDictionary()
        {
            var candidate = ((TopLevel)null).ToStringDictionary();
            Assert.That(candidate, Is.Not.Null);
            Assert.That(candidate.Count, Is.EqualTo(0));
        }

        [Test]
        public void BasicTypesReturnEmptyDictionary()
        {
            var candidate = 3.ToStringDictionary();
            Assert.That(candidate, Is.Not.Null);
            Assert.That(candidate.Count, Is.EqualTo(0));
        }

        [Test]
        public void BasicPropertiesAreAddedToDictionary()
        {
            var candidate = new TopLevel { Int = 3, String = "hello" }.ToStringDictionary();
            Assert.That(candidate, Is.Not.Null);
            Assert.That(candidate["Int"], Is.EqualTo("3"));
            Assert.That(candidate["String"], Is.EqualTo("hello"));
        }

        [Test]
        public void ComplexPropertiesAreAddedToDictionary()
        {
            var candidate = new TopLevel { Child = new Child { Decimal = 0.1m, Double = 1.0 } }.ToStringDictionary();
            Assert.That(candidate, Is.Not.Null);
            Assert.That(candidate["Child.Decimal"], Is.EqualTo("0.1"));
            Assert.That(candidate["Child.Double"], Is.EqualTo("1"));
        }

        [Test]
        public void ConvertibleTypesAreAddedToDictionary()
        {
            var candidate = new Converter().ToStringDictionary();
            Assert.That(candidate["key"], Is.EqualTo("value"));
        }

        [Test]
        public void ToStringDictionaryWithPathingOff()
        {
            var candidate = new TopLevel { Child = new Child { Decimal = 0.1m, Double = 1.0 } }.ToStringDictionary(false);
            Assert.That(candidate, Is.Not.Null);
            Assert.That(candidate["Decimal"], Is.EqualTo("0.1"));
            Assert.That(candidate["Double"], Is.EqualTo("1"));
        }

        [Test]
        public void ConvertersAreAdded()
        {
            var candidate = new TopLevel { Converter = new Converter(), Child = new Child { Converter = new Converter("value2") } }.ToStringDictionary();
            Assert.That(candidate["Child.key"], Is.EqualTo("value2"));
            Assert.That(candidate["key"], Is.EqualTo("value"));
        }

        [Test]
        public void DuplicatesAreNotAddedWhenPathingIsOff() // NOTE this test also shows that properties are handled in order if child was before converter in toplevel then the expected value would be value2.. 
        {
            var candidate = new TopLevel { Converter = new Converter(), Child = new Child { Converter = new Converter("value2") } }.ToStringDictionary(false);
            Assert.That(candidate.ContainsKey("Child.key"), Is.False);
            Assert.That(candidate["key"], Is.EqualTo("value"));
        }

        [Test]
        public void ListsWithListsEtc()
        {
            var candidate = new TopLevel { ChildList = new List<Child> { new Child { Decimal = decimal.Zero, GrandChildList = new List<Child> { new Child { Decimal = decimal.One } } } } }.ToStringDictionary();
            Assert.That(candidate["ChildList.Decimal"], Is.EqualTo("0"));
            Assert.That(candidate["ChildList.GrandChildList.Decimal"], Is.EqualTo("1"));
        }

        [Test]
        public void DatesAreHandledCorrectly()
        {
            var candidate = new TopLevel { DateTime = new DateTime(1, 1, 1, 1, 1, 1), DateTimeOffset = new DateTimeOffset(new DateTime(2, 2, 2, 2, 2, 2), new TimeSpan(0, 2, 0, 0)) }.ToStringDictionary();
            Assert.That(candidate["DateTime"], Is.EqualTo(new DateTime(1, 1, 1, 1, 1, 1).ToString(CultureInfo.InvariantCulture)));
            Assert.That(candidate["DateTimeOffset"], Is.EqualTo(new DateTimeOffset(new DateTime(2, 2, 2, 2, 2, 2), new TimeSpan(0, 2, 0, 0)).ToString()));
        }
    }
}