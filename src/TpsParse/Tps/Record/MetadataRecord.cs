namespace TpsParse.Tps.Record
{
    public class MetadataRecord : TpsRecord
    {
        public MetadataRecord(byte[] data, byte[] header)
            : base(data, header, true)
        {
            AboutType = header[5];
        }

        public byte AboutType { get; }

        public bool IsAboutKeyOrIndex()
        {
            return AboutType < 0xF3;
        }

        public override T Accept<T>(ITpsFileVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}