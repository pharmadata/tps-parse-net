using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record.Fields
{
    public class SignedShortField : FieldDefinitionRecord
    {
        public SignedShortField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            return BitUtil.ToInt16(inputStream);
        }
    }
}