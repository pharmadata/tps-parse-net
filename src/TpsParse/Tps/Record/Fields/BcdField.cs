using System.IO;
using System.Text;

namespace TpsParse.Tps.Record.Fields
{
    public class BcdField : FieldDefinitionRecord
    {
        public BcdField(Stream inputStream)
            : base(inputStream)
        {
            BcdDigitsAfterDecimalPoint = (byte) inputStream.ReadByte();
            BcdLengthOfElement = (byte) inputStream.ReadByte();
        }

        public byte BcdDigitsAfterDecimalPoint { get; }
        public byte BcdLengthOfElement { get; }

        public override object GetValue(Stream inputStream)
        {
            var buffer = new byte[BcdLengthOfElement];
            inputStream.Read(buffer, 0, BcdLengthOfElement);

            var builder = new StringBuilder();
            foreach (var b in buffer)
                builder.AppendFormat("{0:X2}", b);

            var str = builder.ToString();
            var sign = str.Substring(0, 1);
            var number = str.Substring(1);
            if (BcdDigitsAfterDecimalPoint > 0)
            {
                var decimalIndex = number.Length - BcdDigitsAfterDecimalPoint;
                number = number.Substring(0, decimalIndex) + "." + number.Substring(decimalIndex);
            }
            return decimal.Parse((sign != "0" ? "-" : "") + number.TrimStart('0'));
        }
    }
}