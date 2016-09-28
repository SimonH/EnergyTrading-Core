using System;

namespace EnergyTrading.Contracts.Performance
{
    [Serializable]
    public class PerformanceMetrics
    {
        public string MachineName { get; set; }
        public int? TimeMs { get; set; }

        public override string ToString()
        {
            return "Machine : " + (MachineName ?? string.Empty) + ", Time : " + (TimeMs.HasValue ? TimeMs.Value.ToString() : string.Empty);
        }
            

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var metrics = obj as PerformanceMetrics;
            return metrics != null && ToString().Equals(obj.ToString());
        }
    }
}