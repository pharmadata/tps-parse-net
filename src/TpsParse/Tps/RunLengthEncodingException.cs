using System;
using System.Runtime.Serialization;

namespace TpsParse.Tps
{
    [Serializable]
    public class RunLengthEncodingException : Exception
    {
        public RunLengthEncodingException(string message)
            : base(message)
        {
        }

        public RunLengthEncodingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RunLengthEncodingException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}