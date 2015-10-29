using System;
using System.IO;

namespace TpsParse.Tps.Record.Fields
{
    public class FloatField : FieldDefinitionRecord
    {
        public FloatField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            var buffer = new byte[4];
            inputStream.Read(buffer, 0, 4);
            return BitConverter.ToSingle(buffer, 0);
        }
    }
}