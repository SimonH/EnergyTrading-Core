namespace EnergyTrading.Mapping
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class NamedMapperAttribute : Attribute
    {
        public readonly string Name;

        public NamedMapperAttribute(string name)
        {
            this.Name = name;
        }
    }
}