using System;
using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record.Fields
{
    public class TimeField : FieldDefinitionRecord
    {
        public TimeField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            var time = BitUtil.ToInt32(inputStream);
            var mins = (time & 0x00FF0000) >> 16;
            var hours = (time & 0x7F000000) >> 24;
            return new TimeSpan(hours, mins, 0);
        }
    }
}