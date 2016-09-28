using System;

namespace EnergyTrading.Contracts.Performance
{
    [Serializable]
    public class MessageTransportMetrics : MessageMetrics
    {
        public string FromProcessor { get; set; }
        public string ToProcessor { get; set; }
    }
}