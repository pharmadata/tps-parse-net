using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record.Fields
{
    public class UnsignedLongField : FieldDefinitionRecord
    {
        public UnsignedLongField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            return BitUtil.ToUInt32(inputStream);
        }
    }
}