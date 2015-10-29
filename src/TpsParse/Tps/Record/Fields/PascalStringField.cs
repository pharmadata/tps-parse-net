using System.IO;
using System.Text;

namespace TpsParse.Tps.Record.Fields
{
    public class PascalStringField : StringField
    {
        public PascalStringField(Stream inputStream)
            : base(inputStream)
        {
        }

        public override object GetValue(Stream inputStream)
        {
            var len = inputStream.ReadByte();
            var buffer = new byte[len];
            inputStream.Read(buffer, 0, len);
            return Encoding.GetEncoding("iso-8859-1").GetString(buffer);
        }
    }
}