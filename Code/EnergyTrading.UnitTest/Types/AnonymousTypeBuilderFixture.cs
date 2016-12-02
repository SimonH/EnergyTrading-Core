using System.Collections.Generic;
using System.Dynamic;
using EnergyTrading.Types;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.Types
{
    [TestFixture]
    public class AnonymousTypeBuilderFixture
    {
        [Test]
        public void CanCreateSimpleTypeWithProperties()
        {
            var anon = AnonymousTypeBuilder.DefineType().WithProperty("name", "Fred").WithProperty("age", 30033).Instance();
            Assert.That(anon.GetType().GetProperty("name").GetValue(anon), Is.EqualTo("Fred"));
            Assert.That(anon.GetType().GetProperty("age").GetValue(anon), Is.EqualTo(30033));
        }

        [Test]
        public void CanCreateFromAnExpandoObjectDictionary()
        {
            var source = new Dictionary<string, object>
            {
                { "name", "Fred" },
                { "age", 30033 }
            };
            var anon = AnonymousTypeBuilder.FromDictionary(source);
            Assert.That(anon.GetType().GetProperty("name").GetValue(anon), Is.EqualTo("Fred"));
            Assert.That(anon.GetType().GetProperty("age").GetValue(anon), Is.EqualTo(30033));
        }
    }
}