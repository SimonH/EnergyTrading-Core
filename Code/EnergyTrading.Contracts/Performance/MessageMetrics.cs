using System;

namespace EnergyTrading.Contracts.Performance
{
    [Serializable]
    public class MessageMetrics : PerformanceMetrics
    {
        public string MessageType { get; set; }
        public string MessageTypeName { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            var start = base.ToString();
            var end = " MessageType : " + MessageType + ", MessageTypeName : " + MessageTypeName;
            return start + end;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var metrics = obj as MessageMetrics;
            return metrics != null && ToString().Equals(obj.ToString());
        }
    }
}