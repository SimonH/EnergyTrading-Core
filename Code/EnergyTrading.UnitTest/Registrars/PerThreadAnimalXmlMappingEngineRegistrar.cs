﻿using System;
using System.Collections.Generic;
using EnergyTrading.Registrars;
using Microsoft.Practices.Unity;

namespace EnergyTrading.UnitTest.Registrars
{
    public class PerThreadAnimalXmlMappingEngineRegistrar : PerThreadVersionedXmlMappingEngineRegistrar
    {
        public PerThreadAnimalXmlMappingEngineRegistrar()
        {
            this.SchemaName = "Animal";
        }

        public static readonly List<MapperArea> V1 = new List<MapperArea>
        {
            new MapperArea(),
            new MapperArea { Area = "Common", Version = 1 }
        };

        public static readonly List<MapperArea> V2 = new List<MapperArea>
        {
            new MapperArea(),
            new MapperArea { Area = "Common", Version = 2 }
        };

        public static readonly List<MapperArea> V2R1 = new List<MapperArea>
        {
            new MapperArea(),
            new MapperArea { Area = "Common", Version = 2, AllowedMinorVersions = { 2.1 } }
        };

        public void Register(IUnityContainer container)
        {
            this.RegisterVersionedEngine(container, new Version(1, 0), V1);
            this.RegisterVersionedEngine(container, new Version(2, 0), V2);
            this.RegisterVersionedEngine(container, new Version(2, 1), V2R1);
        }

        protected override bool IsVersionedMapper(Type type, string area, double version)
        {
            var ns = "EnergyTrading.UnitTest.Registrars.Maps" + this.ToVersionString(area, version);

            return type.Namespace == ns;
        }
    }
}