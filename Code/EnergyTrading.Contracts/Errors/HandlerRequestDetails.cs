using System;

namespace EnergyTrading.Contracts.Errors
{
    public class HandlerRequestDetails
    {
        public string Name { get; set; }
        public DateTime Since { get; set; }

        public string[] ValidErrorCodes { get; set; }
    }
}