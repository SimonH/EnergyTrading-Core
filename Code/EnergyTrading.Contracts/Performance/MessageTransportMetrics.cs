using System;

namespace EnergyTrading.Contracts.Performance
{
    [Serializable]
    public class MessageTransportMetrics : MessageMetrics
    {
        public string FromMachineName { get; set; }
        public string FromProcessor { get; set; }
        public string ToProcessor { get; set; }

        public override string ToString()
        {
            var start = base.ToString();
            var end = " FromMachineName : " + FromMachineName + ", FromProcessor : " + FromProcessor + ", ToProcessor : " + ToProcessor;
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

            var metrics = obj as MessageTransportMetrics;
            return metrics != null && ToString().Equals(obj.ToString());
        }
    }
}