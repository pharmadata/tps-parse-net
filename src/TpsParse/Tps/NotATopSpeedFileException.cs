using System;
using System.Runtime.Serialization;

namespace TpsParse.Tps
{
    [Serializable]
    public class NotATopSpeedFileException : Exception
    {
        public NotATopSpeedFileException(string message)
            : base(message)
        {
        }

        protected NotATopSpeedFileException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}