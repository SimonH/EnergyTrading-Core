using System;
using System.Collections;
using EnergyTrading.Data;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.Data
{
    [TestFixture]
    public class DataConversionExtensionsFixture
    {
        public static IEnumerable FromRowVersionSource
        {
            get
            {
                yield return new TestCaseData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0UL).SetName("0");
                yield return new TestCaseData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, 1UL).SetName("1");
                yield return new TestCaseData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01 }, 257UL).SetName("257");
                yield return new TestCaseData(new byte[] { 0x9E, 0x00, 0x00, 0x56, 0x00, 0x00, 0x78, 0x00 }, 0x9E00005600007800).SetName("generalbytearray");
            }
        }

        [Test]
        public void InvalidArray()
        {
            Assert.That(() => new byte[10].FromRowVersion(), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCaseSource(nameof(FromRowVersionSource))]
        public void FromRowVersion(byte[] source, ulong expected)
        {
            Assert.That(source.FromRowVersion(), Is.EqualTo(expected));
        }
    }
}