namespace EnergyTrading.Configuration
{
    public class NamedItemsCollection : NamedConfigElementCollection<NamedConfigElement>
    {
        protected override string ElementName => "item";
    }
}