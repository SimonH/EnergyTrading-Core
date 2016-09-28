using System;

namespace EnergyTrading.Contracts.Performance
{
    [Serializable]
    public class PerformanceMetrics
    {
        public string MachineName { get; set; }
        public int? TimeMs { get; set; }
    }
}