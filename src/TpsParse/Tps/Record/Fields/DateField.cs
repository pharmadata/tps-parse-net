using System;
using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record.Fields
{
    public class DateField : FieldDefinitionRecord
    {
        public DateField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            var date = BitUtil.ToUInt32(inputStream);
            if (date == 0)
                return null;

            var years = (int) ((date & 0xFFFF0000) >> 16);
            var months = (int) ((date & 0x0000FF00) >> 8);
            var days = (int) (date & 0x000000FF);

            return new DateTime(years, months, days, 0, 0, 0, DateTimeKind.Unspecified);
        }
    }
}