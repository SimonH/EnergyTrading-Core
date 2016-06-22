namespace EnergyTrading.UnitTest.Extensions
{
    using System;
    using System.Collections.Generic;

    using EnergyTrading.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class ExceptionExtensionsFixture
    {
        [Test]
        public void AddThrownNullListFuncReturns()
        {
            var count = 0;
            ExceptionExtensions.AddThrown(null, () => count++);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void ExceptionsAreThrownIfListIsNull()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExceptionExtensions.AddThrown(null, () => { throw new ArgumentOutOfRangeException(); }));
        }

        [Test]
        public void ThrownExceptionsAreAddedToListByDefault()
        {
            var ex = new ArgumentOutOfRangeException();
            var list = new List<Exception>();
            list.AddThrown(() => { throw ex; });
            Assert.AreEqual(1, list.Count);
            Assert.AreSame(ex, list[0]);
        }

        [Test]
        public void HandledExceptionTypesCanBeRestricted()
        {
            var handledTypes = new List<Type> { typeof(ArgumentOutOfRangeException) };
            var ex = new ArgumentOutOfRangeException();
            var list = new List<Exception>();
            list.AddThrown(() => { throw ex; }, handledTypes);
            Assert.AreEqual(1, list.Count);
            Assert.AreSame(ex, list[0]);
            Assert.Throws<ArgumentNullException>(() => list.AddThrown(() => { throw new ArgumentNullException(); }, handledTypes));
        }
    }
}