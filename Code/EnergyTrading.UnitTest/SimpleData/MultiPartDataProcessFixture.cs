﻿namespace EnergyTrading.UnitTest.SimpleData
{
    using System;
    using System.Text;

    using EnergyTrading.Data.SimpleData;

    using NUnit.Framework;

    [TestFixture]
    public class MultiPartDataProcessFixture
    {
        private static readonly string TestData = "here is my test data, hopefully it will all get stored";

        [Test]
        public void ConstructWithoutProcessor()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new MultiPartDataProcess(null));
// ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void ConstructWithZeroPartSize()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new MultiPartDataProcess((x, y, z) => true, null, null, 0));
// ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void ConstructWithNegativePartSize()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new MultiPartDataProcess((x, y, z) => true, null, null, -1));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void TestSingleChunkIsProcessed()
        {
            var sb = new StringBuilder();
            var processor = new MultiPartDataProcess((x, y, z) =>
                {
                    Assert.AreEqual(1, y);
                    Assert.AreEqual(1, z);
                    sb.Append(x);
                    return true;
                });
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsTrue(candidate);
        }

        [Test]
        public void MultiplePartsAreProcessed()
        {
            var sb = new StringBuilder();
            var count = 1;
            var processor = new MultiPartDataProcess((x, y, z) =>
            {
                Assert.AreEqual(count++, y);
                sb.Append(x);
                return true;
            }, null, null, 3);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsTrue(candidate);
        }

        [Test]
        public void PartsAreNotProcessedIfStartProcessFails()
        {
            var processor = new MultiPartDataProcess((x, y, z) =>
                                    {
                                        Assert.Fail();
                                        return false;
                                    },
                                    x => false);
            var candidate = processor.ProcessData(TestData);
            Assert.IsFalse(candidate);
        }

        [Test]
        public void PartsAreProcessedIfStartProcessSucceeds()
        {
            var sb = new StringBuilder();
            var processor = new MultiPartDataProcess((x, y, z) =>
                                    {
                                        sb.Append(x);
                                        return true;
                                    },
                                    x => true);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsTrue(candidate);
        }

        [Test]
        public void StopsWithFalseIfPartFailsToProcess()
        {
            var sb = new StringBuilder();
            var count = 1;
            var processor = new MultiPartDataProcess((x, y, z) =>
            {
                Assert.AreEqual(count++, y);
                sb.Append(x);
                return y != 3;
            }, null, null, 3);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData.Substring(0, 3*3), sb.ToString());
            Assert.IsFalse(candidate);
        }

        [Test]
        public void ReturnsFalseIfEndProcessFails()
        {
            var sb = new StringBuilder();
            var processor = new MultiPartDataProcess((x, y, z) =>
                                    {
                                        sb.Append(x);
                                        return true;
                                    },
                                    x => true,
                                    x => false);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsFalse(candidate);
        }

        [Test]
        public void ReturnsTrueIfEndProcessSucceeds()
        {
            var sb = new StringBuilder();
            var processor = new MultiPartDataProcess((x, y, z) =>
            {
                sb.Append(x);
                return true;
            },
                                    x => true,
                                    x => true);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsTrue(candidate);
        }
    }
}