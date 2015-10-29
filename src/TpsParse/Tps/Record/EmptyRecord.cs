namespace TpsParse.Tps.Record
{
    public class EmptyRecord : TpsRecord
    {
        public EmptyRecord(byte[] data, byte[] header)
            : base(data, header, false)
        {
        }

        public override T Accept<T>(ITpsFileVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}