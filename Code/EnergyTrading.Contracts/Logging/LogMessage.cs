using System;
using System.Runtime.Serialization;

namespace EnergyTrading.Contracts.Logging
{
    [Serializable]
    [DataContract]
    public class LogMessage
    {
        [DataMember]
        public string CreatingType { get; set; }

        [DataMember]
        public string Level { get; set; }

        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// When contract is used objects in the params must be serializable using the chosen method (e.g. .NetXml, Json, custom serialization logic)
        /// </summary>
        [DataMember]
        public object[] Params { get; set; } 

        /// <summary>
        /// using a LogMessageException here keeps information while ensuring that the object remains serializable (as many exception types are not serializable despite the guidelines)
        /// use LogMessageException.FromException(Exception exception) to obtain a LogMessageException
        /// </summary>
        [DataMember]
        public LogMessageException Exception { get; set; }
    }
}