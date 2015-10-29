using System.IO;
using System.Text;

namespace TpsParse.Tps.Record.Fields
{
    public class FixedLengthStringField : StringField
    {
        public FixedLengthStringField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            var buffer = new byte[Length];
            inputStream.Read(buffer, 0, Length);
            return Encoding.GetEncoding("iso-8859-1").GetString(buffer);
        }
    }
}