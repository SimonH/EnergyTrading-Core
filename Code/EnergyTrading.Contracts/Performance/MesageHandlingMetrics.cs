using System;

namespace EnergyTrading.Contracts.Performance
{
    [Serializable]
    public class MesageHandlingMetrics : MessageMetrics
    {
        public string Processor { get; set; }
        public int? TimeInHandlerMs { get; set; }

        public override string ToString()
        {
            var start = base.ToString();
            var end = " Processor : " + Processor + ", TimeInHandlerMs : " + (TimeInHandlerMs.HasValue ? TimeInHandlerMs.Value.ToString() : string.Empty);
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

            var metrics = obj as MesageHandlingMetrics;
            return metrics != null && ToString().Equals(obj.ToString());
        }
    }
}