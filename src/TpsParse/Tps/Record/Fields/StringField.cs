using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record.Fields
{
    public abstract class StringField : FieldDefinitionRecord
    {
        public StringField(Stream inputStream)
            : base(inputStream)
        {
            StringLength = BitUtil.ToInt16(inputStream);
            StringMask = BitUtil.ZeroTerminatedString(inputStream);
            if (StringMask.Length == 0)
                inputStream.ReadByte();
        }

        public short StringLength { get; private set; }
        public string StringMask { get; }
    }
}