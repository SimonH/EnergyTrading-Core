﻿namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public class Entity : INullableProperties
    {
        private int total;

        public Entity()
        {
            this.NullProperties = new NullPropertyBag();
            NullProperties.Loading = true;
            // NB Needed if we need the default to be "null" i.e. not emitted
            this.NullProperties["Total"] = true;
            Value = int.MinValue;
            Total = int.MinValue;
            NullProperties.Loading = false;
        }

        public NullPropertyBag NullProperties { get; private set; }

        public string Id { get; set; }

        public string Id2 { get; set; }

        public string Name { get; set; }

        public string Name2 { get; set; }

        public int Value { get; set; }

        public int Total
        {
            get
            {
                return this.total;
            }
            set
            {
                this.total = value;
                this.NullProperties.Assigned("Total");
            }
        }
    }
}