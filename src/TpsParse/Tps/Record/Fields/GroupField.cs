using System.IO;

namespace TpsParse.Tps.Record.Fields
{
    public class GroupField : FieldDefinitionRecord
    {
        public GroupField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            var buffer = new byte[Length];
            inputStream.Read(buffer, 0, Length);
            return buffer;
        }
    }
}