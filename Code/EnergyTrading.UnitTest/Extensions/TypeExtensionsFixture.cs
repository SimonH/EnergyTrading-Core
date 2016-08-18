using System.Collections;
using System.Collections.Generic;
using EnergyTrading.Contracts.Errors;
using EnergyTrading.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.Extensions
{
    [TestFixture]
    public class TypeExtensionsFixture
    {
        public static IEnumerable AssemblyQualifiedNameWithoutVersionSource
        {
            get
            {
                yield return new TestCaseData(new ErrorMessage(), "EnergyTrading.Contracts.Errors.ErrorMessage, EnergyTrading.Contracts").SetName("ForErrorMessage");
            }
        }

        [Test]
        [TestCaseSource("AssemblyQualifiedNameWithoutVersionSource")]
        public void AssemblyQualifiedNameWithoutVersion(object source, string expected)
        {
            Assert.That(source.GetType().AssemblyQualifiedNameWithoutVersion(), Is.EqualTo(expected));
        }
    }
}