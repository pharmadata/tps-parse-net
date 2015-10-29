using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record.Fields
{
    public class SignedLongField : FieldDefinitionRecord
    {
        public SignedLongField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            return BitUtil.ToInt32(inputStream);
        }
    }
}