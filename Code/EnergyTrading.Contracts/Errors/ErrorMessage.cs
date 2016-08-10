using System;
using System.Collections.Generic;

namespace EnergyTrading.Contracts.Errors
{
    public class ErrorMessage
    {
        /// <summary>
        /// Unique identifier for the error.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// UTC time when the error occurred
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// The broad category of the error (e.g. Business / Technical)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Unique code identifying the exact error (normally namespaced to prevent clashes)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The severity of the error
        /// </summary>
        public string Severity { get; set; }

        /// <summary>
        /// text message describing the detail of the error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Opaque data related to the error (e.g. serialized payloads, source data) normally interpretted and used at the point of handling the error
        /// </summary>
        public IEnumerable<ReferenceData> ReferenceDatas { get; set; }

        /// <summary>
        /// Additional properties as string:key / string:value pairs
        /// </summary>
        public IDictionary<string, string> AdditionalProperties { get; set; }
    }
}