using EnergyTrading.Types;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.Types
{
    [TestFixture]
    public class AnonymousTypeParserFixture
    {
        [Test]
        public void ToDictionary()
        {
            var candidate = new { name = "Fred", age = 30033 }.ToDictionary();
            Assert.That(candidate["name"], Is.EqualTo("Fred"));
            Assert.That(candidate["age"], Is.EqualTo(30033));
        }

        [Test]
        public void ToDynamic()
        {
            var candidate = new { name = "Fred", age = 30033 }.ToDynamic();
            Assert.That(candidate.name, Is.EqualTo("Fred"));
            Assert.That(candidate.age, Is.EqualTo(30033));

        }
    }
}