namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public class ChildChildModelMapper : Mapper<Child, ChildModel>
    {
        public override void Map(Child source, ChildModel destination)
        {
            destination.Id = source.Id;
            destination.Value = source.Value;
        }
    }
}