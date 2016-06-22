﻿namespace EnergyTrading.UnitTest.Mapping
{
    using System;

    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class XPathProcessorFixture : Fixture
    {
        private static int warnCount;

        protected string Xml { get; set; }

        protected override void OnSetup()
        {
            // NB See ConfigLoggerFactory for how the mock logger is created.
            AssemblyLoggerProvider.MockLogger.Setup(x => x.Warn(It.IsAny<string>())).Callback(() => ++warnCount);
            warnCount = 0;
 
            Xml = @"<Fred xmlns='http://sample.com' xmlns:a='http://sample.com/a'>
                                        <Jim xmlns='http://test.com'>a</Jim>
                                        <Bob>b</Bob>
                                     </Fred>";
        }

        protected override void OnTearDown()
        {
            AssemblyLoggerProvider.MockLogger.Setup(x => x.Warn(It.IsAny<string>())).Callback(() => { });
        }

        [Test]
        public void PushSingleNameSpaceReportsCorrectly()
        {
            var processor = XPathProcessor(Xml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("test", "http://test.com");

            processor.Push("Fred", "http://sample.com");

            Assert.AreEqual(@"http://sample.com", processor.CurrentNamespace);
            Assert.AreEqual(@"/sample:Fred/", processor.CurrentPath);
        }

        [Test]
        public void PushPrefixTakesPriorityOverNamespace()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>a</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://test.com", "sample");

            Assert.AreEqual(@"http://test.com", processor.CurrentNamespace);
            Assert.AreEqual(@"/sample:Fred/", processor.CurrentPath);
            Assert.IsNotNull(processor.CurrentNode());
        }

        [Test]
        public void PushPopGetsCorrectNamespaceAndPath()
        {
            var processor = XPathProcessor(Xml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("test", "http://test.com");

            processor.Push("Fred", "http://sample.com");
            processor.Push("Jim", "http://test.com");
            processor.Pop();

            Assert.AreEqual(@"http://sample.com", processor.CurrentNamespace);
            Assert.AreEqual(@"/sample:Fred/", processor.CurrentPath);
        }

        [Test]
        public void ProcessNodesStackSameParentSameChild()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Jim>
                                            <Dave>10</Dave>
                                        </Jim>
                                        <Jim>
                                            <Dave>30</Dave>
                                        </Jim>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            processor.Push("Jim", index: 2);

            var candidate = processor.ToInt("Dave");
            Assert.AreEqual(30, candidate, "Second Dave differs");
            processor.Pop();

            processor.Push("Jim", index: 1);
            candidate = processor.ToInt("Dave");
            Assert.AreEqual(10, candidate, "First Dave differs");
        }

        [Test]
        public void ProcessNodesStackDifferentParentSameChild()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Jim>
                                            <Dave>
                                                <James>10</James>
                                            </Dave>
                                            <Dave>
                                                <James>20</James>
                                            </Dave>
                                        </Jim>
                                        <Bob>
                                            <Dave>
                                                <James>30</James>
                                            </Dave>
                                            <Dave>
                                                <James>40</James>
                                            </Dave>
                                        </Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            processor.Push("Jim");

            processor.Push("Dave", index: 1);
            var candidate = processor.ToInt("James");
            Assert.AreEqual(10, candidate, "First James differs");
            processor.Pop();

            processor.Push("Dave", index: 2);
            candidate = processor.ToInt("James");
            Assert.AreEqual(20, candidate, "Second James differs");
            processor.Pop();
            processor.Pop();

            processor.Push("Bob");
            processor.Push("Dave", index: 2);
            candidate = processor.ToInt("James");
            Assert.AreEqual(40, candidate, "Fourth James differs");
            processor.Pop();

            processor.Push("Dave", index: 1);
            candidate = processor.ToInt("James");
            Assert.AreEqual(30, candidate, "Third James differs");
            processor.Pop();
        }

        [Test]
        public void HasElementOrAttributeShouldReturnTrueIfElementExistsOnCurrentNode()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:a='http://sample.com/a'>
                                        <Jim xmlns='http://test.com'>a</Jim>
                                        <Bob></Bob>
                                        <Dave />
                                     </Fred>";
            var processor = XPathProcessor(TestXml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("test", "http://test.com");

            processor.Push("Fred", "http://sample.com");
            var aExists = processor.HasAttribute("a", "xsi");
            var jimExist = processor.HasElement("Jim", "test");
            var bobExist = processor.HasElement("Bob");
            var daveExist = processor.HasElement("Dave");

            Assert.IsTrue(aExists, "Attribute a doesn't exist");
            Assert.IsTrue(jimExist, "Jim doesn't exist");
            Assert.IsTrue(bobExist, "Bob doesn't exist");
            Assert.IsTrue(daveExist, "Dave doesn't exist");
        }

        [Test]
        public void HasElementOrAttributeShouldReturnFalseIfElementNotExistsOnCurrentNode()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' xmlns:a='http://sample.com/a'>
                                        <Jim xmlns='http://test.com'>a</Jim>
                                        <Bob></Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("testabc", "http://testabc.com");

            processor.Push("Fred", "http://sample.com");
            var jimInDifferentNamespaceExist = processor.HasElement("Jim", "testabc");
            var bobABCExist = processor.HasElement("BobABC");

            Assert.IsFalse(jimInDifferentNamespaceExist, "Jim exists");
            Assert.IsFalse(bobABCExist, "Bob exists");
        }

        [Test]
        public void HasElementOrAttributeHandlesIndex()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Jim>
                                            <Dave>
                                                <James>10</James>
                                            </Dave>
                                            <Dave>
                                                <James>20</James>
                                            </Dave>
                                        </Jim>
                                        <Bob>
                                            <Dave>
                                                <James>30</James>
                                            </Dave>
                                            <Dave>
                                                <James>40</James>
                                            </Dave>
                                        </Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            processor.Push("Jim");

            Assert.IsTrue(processor.HasElement("Dave", index: 1));
            Assert.IsTrue(processor.HasElement("Dave", index: 2));

            processor.Pop();
            processor.Pop();
        }

        [Test]
        public void IsNullShouldReturnFalseIfElementExistsOnCurrentNode()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' xmlns:a='http://sample.com/a'>
                                        <Jim xmlns='http://test.com'>a</Jim>
                                        <Bob></Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("test", "http://test.com");

            processor.Push("Fred", "http://sample.com");
            var jimExist = processor.IsNull("Jim", prefix: "test");
            var bobExist = processor.IsNull("Bob");

            Assert.IsFalse(jimExist, "Jim IsNull");
            Assert.IsFalse(bobExist, "Bob IsNull");
        }

        [Test]
        public void IsNullShouldReturnTrueForEmptyNode()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' xmlns:a='http://sample.com/a'>
                                        <Dave />
                                     </Fred>";
            var processor = XPathProcessor(TestXml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("test", "http://test.com");

            processor.Push("Fred", "http://sample.com");
            var daveExist = processor.IsNull("Dave");

            Assert.IsTrue(daveExist, "Dave not IsNull");
        }

        [Test]
        public void IsNullShouldReturnTrueIfElementNotExistsOnCurrentNode()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' xmlns:a='http://sample.com/a'>
                                        <Jim xmlns='http://test.com'>a</Jim>
                                        <Bob></Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("testabc", "http://testabc.com");

            processor.Push("Fred", "http://sample.com");
            var jimInDifferentNamespaceExist = processor.IsNull("Jim", prefix: "testabc");
            var bobABCExist = processor.IsNull("BobABC");

            Assert.IsTrue(jimInDifferentNamespaceExist);
            Assert.IsTrue(bobABCExist);
        }

        [Test]
        public void ToValueReturnsCustomDefaultWhenNoNodePresent()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' xmlns:a='http://sample.com/a'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("testabc", "http://testabc.com");

            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToInt("Bob", defaultValue: -1);

            Assert.AreEqual(-1, candidate, "Values differ");
        }

        [Test]
        public void ToValueReturnsCustomDefaultWhenNodeEmpty()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' xmlns:a='http://sample.com/a'>
                                        <Bob />
                                     </Fred>";
            var processor = XPathProcessor(TestXml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("testabc", "http://testabc.com");

            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToInt("Bob", defaultValue: -1);

            Assert.AreEqual(-1, candidate, "Values differ");
        }

        [Test]
        public void ToStringReturnsValue()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>a</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToString("Bob");
            Assert.AreEqual("a", candidate);
        }

        [Test]
        public void ToStringWithPrefixReturnsValue()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>a</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToString("Bob", "sample");
            Assert.AreEqual("a", candidate);
        }

        [Test]
        public void ToStringAttributeReturnsValue()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='a'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToString("Bob", isAttribute: true);
            Assert.AreEqual("a", candidate);
        }

        [Test]
        public void ToStringPrefixedAttributeReturnsValue()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' xmlns:test='http://sample.com/test' test:Bob='a'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("test", "http://sample.com/test");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToString("Bob", "test", isAttribute: true);
            Assert.AreEqual("a", candidate);
        }

        [Test]
        public void ToIntParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>10</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToInt("Bob");
            Assert.AreEqual(10, candidate);
        }

        [Test]
        public void ToIntAttributeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='10'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToInt("Bob", isAttribute: true);
            Assert.AreEqual(10, candidate);
        }

        [Test]
        public void ToIntParsesChildArrayCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>
                                            <Jim>
                                                <Dave>10</Dave>
                                            </Jim>
                                            <Jim>
                                                <Dave>20</Dave>
                                            </Jim>
                                        </Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            processor.Push("Bob");
            
            processor.Push("Jim", "http://sample.com", index: 1);
            var c1 = processor.ToInt("Dave");
            Assert.AreEqual(10, c1, "First Dave differs");
            processor.Pop();

            processor.Push("Jim", "http://sample.com", index: 2);
            var c2 = processor.ToInt("Dave");
            Assert.AreEqual(20, c2, "Second Dave differs");            
        }

        [Test]
        public void ToLongParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>10000</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToLong("Bob");
            Assert.AreEqual(10000, candidate);
        }

        [Test]
        public void ToLongAttributeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='10000'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToLong("Bob", isAttribute: true);
            Assert.AreEqual(10000, candidate);
        }

        [Test]
        public void ToEnumParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>CIF</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToEnum<IncoTerms>("Bob");
            Assert.AreEqual(IncoTerms.CIF, candidate);
        }

        [Test]
        public void ToEnumAttributeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='CIF'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToEnum<IncoTerms>("Bob", isAttribute: true);
            Assert.AreEqual(IncoTerms.CIF, candidate);
        }

        [Test]
        public void ToBoolParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>true</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToBool("Bob");
            Assert.AreEqual(true, candidate);
        }

        [Test]
        public void ToBoolAttributeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='true'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToBool("Bob", isAttribute: true);
            Assert.AreEqual(true, candidate);
        }

        [Test]
        public void ToDecimalLogsWarningIfValueContainsAComma()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2,000</Bob>
                                     </Fred>"; 
            var startwarncount = warnCount;
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDecimal("Bob");
            Assert.AreEqual(2000m, candidate);
            Assert.AreEqual(startwarncount + 1, warnCount);
        }

        [Test]
        public void ToDecimalLogsWarningIfValueContainsAnExponent()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2.0E4</Bob>
                                     </Fred>"; 
            var startwarncount = warnCount;
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDecimal("Bob");
            Assert.AreEqual(20000m, candidate);
            Assert.AreEqual(startwarncount + 1, warnCount);
        }

        [Test]
        public void ToDecimalFailsForWrongFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2.00,0</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            Assert.Throws<FormatException>(() => processor.ToDecimal("Bob"));
        }

        [Test]
        public void ToDecimalPassesForScientificNotation()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2.0E4</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var canditate = processor.ToDecimal("Bob");
            Assert.AreEqual(20000.0m, canditate);
        }

        [Test]
        public void ToDecimalParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>22700.33</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDecimal("Bob");
            Assert.AreEqual(22700.33m, candidate);
        }

        [Test]
        public void ToDecimalAttributeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='22700.33'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDecimal("Bob", isAttribute: true);
            Assert.AreEqual(22700.33m, candidate);
        }

        [Test]
        public void ToFloatLogsWarningIfValueContainsAComma()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2,000</Bob>
                                     </Fred>";
            var startwarncount = warnCount;

            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToFloat("Bob");
            Assert.AreEqual(2000f, candidate);
            Assert.AreEqual(startwarncount + 1, warnCount);
        }

        [Test]
        public void ToFloatFailsForWrongFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2.00,0</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            Assert.Throws<FormatException>(() => processor.ToFloat("Bob"));
        }

        [Test]
        public void ToFloatParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>22700.33</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToFloat("Bob");
            Assert.AreEqual(22700.33f, candidate);
        }

        [Test]
        public void ToFloatAttributeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='22700.33'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToFloat("Bob", isAttribute: true);
            Assert.AreEqual(22700.33f, candidate);
        }

        [Test]
        public void ToFloatParsesScientificNotation()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2.0E3</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            var startwarncount = warnCount;
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToFloat("Bob");
            Assert.AreEqual(2000.0f, candidate);
            Assert.AreEqual(startwarncount, warnCount);
        }

        [Test]
        public void ToDoubleLogsWarningIfValueContainsAComma()
        {
            var startwarncount = warnCount;
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2,000</Bob>
                                     </Fred>";

            var processor = XPathProcessor(TestXml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDouble("Bob");
            Assert.AreEqual(2000, candidate);
            Assert.AreEqual(startwarncount + 1, warnCount);
        }

        [Test]
        public void ToDoubleFailsForWrongFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2.00,0</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            Assert.Throws<FormatException>(() => processor.ToDouble("Bob"));
        }

        [Test]
        public void ToDoubleParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>22700.33</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDouble("Bob");
            Assert.AreEqual(22700.33, candidate);
        }

        [Test]
        public void ToDoubleAttributeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='22700.33'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDouble("Bob", isAttribute: true);
            Assert.AreEqual(22700.33, candidate);
        }

        [Test]
        public void ToDoubleParsesScientificNotation()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2.0E3</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);
            var startwarncount = warnCount;
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDouble("Bob");
            Assert.AreEqual(2000, candidate);
            Assert.AreEqual(startwarncount, warnCount);
        }

        [Test]
        public void ToDateTimeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2012-07-15T05:12:34Z</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDateTime("Bob");
            Assert.AreEqual(new DateTime(2012, 7, 15, 5, 12, 34), candidate);
        }

        [Test]
        public void ToDateTimeAttributeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='2012-07-15T05:12:34Z'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDateTime("Bob", isAttribute: true);
            Assert.AreEqual(new DateTime(2012, 7, 15, 5, 12, 34), candidate);
        }

        [Test]
        public void ToDateTimeOffsetParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com'>
                                        <Bob>2012-07-15T05:12:34Z</Bob>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDateTimeOffset("Bob");
            Assert.AreEqual(new DateTimeOffset(2012, 7, 15, 5, 12, 34, new TimeSpan()), candidate);
        }

        [Test]
        public void ToDateTimeOffsetAttributeParsesCorrectFormat()
        {
            const string TestXml = @"<Fred xmlns='http://sample.com' Bob='2012-07-15T05:12:34Z'>
                                     </Fred>";
            var processor = XPathProcessor(TestXml);

            processor.RegisterNamespace("sample", "http://sample.com");
            processor.Push("Fred", "http://sample.com");
            var candidate = processor.ToDateTimeOffset("Bob", isAttribute: true);
            Assert.AreEqual(new DateTimeOffset(2012, 7, 15, 5, 12, 34, new TimeSpan()), candidate);
        }

        protected virtual XPathProcessor XPathProcessor(string xml)
        {
            var processor = new XPathProcessor();
            processor.Initialize(xml);
            return processor;
        }
    }
}