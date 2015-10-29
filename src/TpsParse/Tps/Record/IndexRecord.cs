namespace TpsParse.Tps.Record
{
    public class IndexRecord : TpsRecord
    {
        public IndexRecord(byte[] data, byte[] header)
            : base(data, header, true)
        {
            IndexNumber = header[4];
        }

        public int IndexNumber { get; private set; }

        public override T Accept<T>(ITpsFileVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}