using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record.Fields
{
    public class UnsignedShortField : FieldDefinitionRecord
    {
        public UnsignedShortField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            return BitUtil.ToUInt16(inputStream);
        }
    }
}