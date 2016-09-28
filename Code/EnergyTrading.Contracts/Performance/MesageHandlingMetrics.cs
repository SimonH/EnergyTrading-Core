using System;

namespace EnergyTrading.Contracts.Performance
{
    [Serializable]
    public class MesageHandlingMetrics : MessageMetrics
    {
        public string Processor { get; set; }
        public int? TimeInHandlerMs { get; set; }
    }
}