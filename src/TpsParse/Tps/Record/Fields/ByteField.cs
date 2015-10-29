using System.IO;

namespace TpsParse.Tps.Record.Fields
{
    public class ByteField : FieldDefinitionRecord
    {
        public ByteField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            return (byte) inputStream.ReadByte();
        }
    }
}