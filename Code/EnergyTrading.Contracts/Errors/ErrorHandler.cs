using System;

namespace EnergyTrading.Contracts.Errors
{
    /// <summary>
    /// Error handler contract.
    /// </summary>
    public class ErrorHandler
    {
        /// <summary>
        /// Unique identifier for the error handler.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique identifier for the related error.
        /// </summary>
        public string ErrorId { get; set; }

        /// <summary>
        /// Error handler name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Error handling status (e.g. Processing, Failed, Completed)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Indicates when the handler status was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Indicates how many attempts have been made to handle the error.
        /// </summary>
        public int Retries { get; set; }
    }
}
