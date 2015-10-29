using System.Text;
using TpsParse.Util;

namespace TpsParse.Tps.Record
{
    public class TableNameRecord : TpsRecord
    {
        public TableNameRecord(byte[] data, byte[] header)
            : base(data, header, false)
        {
            Name = Encoding.GetEncoding("iso-8859-1").GetString(header, 1, header.Length - 1);
            TableNumber = BitUtil.ToInt32(data, 0, false);
        }

        public string Name { get; }

        public override T Accept<T>(ITpsFileVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}