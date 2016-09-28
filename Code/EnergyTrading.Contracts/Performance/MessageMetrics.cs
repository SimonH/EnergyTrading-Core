using System;

namespace EnergyTrading.Contracts.Performance
{
    [Serializable]
    public class MessageMetrics : PerformanceMetrics
    {
        public string MessageType { get; set; }
        public string MessageTypeName { get; set; }
        public string Message { get; set; }
    }
}