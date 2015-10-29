using System;
using System.IO;

namespace TpsParse.Tps.Record.Fields
{
    public class DoubleField : FieldDefinitionRecord
    {
        public DoubleField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            var buffer = new byte[8];
            inputStream.Read(buffer, 0, 8);
            return BitConverter.ToDouble(buffer, 0);
        }
    }
}