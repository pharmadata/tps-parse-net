using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record.Fields
{
    public class ZeroTerminatedStringField : StringField
    {
        public ZeroTerminatedStringField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            return BitUtil.ZeroTerminatedString(inputStream);
        }
    }
}