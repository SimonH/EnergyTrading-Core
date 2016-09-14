using System.Configuration;
using EnergyTrading.Configuration;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.Configuration
{
    [TestFixture]
    public class NamedItemsSectionTests
    {
        [Test]
        public void CanReadSection()
        {
            var section = ConfigurationManager.GetSection("namedItemsSection") as NamedItemsSection;
            Assert.That(section, Is.Not.Null);
            Assert.That(section.Items, Is.Not.Null);
            Assert.That(section.Items.Count, Is.EqualTo(4));
            Assert.That(section.Items[0].Name, Is.EqualTo("item1"));
            Assert.That(section.Items[3].Name, Is.EqualTo("item4"));
        }

        [Test]
        public void ToStringList()
        {
            var section = ConfigurationManager.GetSection("namedItemsSection") as NamedItemsSection;
            Assert.That(section, Is.Not.Null);
            var list = section.ToStringList();
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo("item1"));
            Assert.That(list[3], Is.EqualTo("item4"));
        }
    }
}