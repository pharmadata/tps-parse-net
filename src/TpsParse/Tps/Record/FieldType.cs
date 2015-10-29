namespace TpsParse.Tps.Record
{
    public enum FieldType : byte
    {
        Byte = 1,
        SignedShort = 2,
        UnsignedShort = 3,
        Date = 4,
        Time = 5,
        SignedLong = 6,
        UnsignedLong = 7,
        Float = 8,
        Double = 9,
        Bcd = 0x0A,
        FixedLengthString = 0x12,
        ZeroTerminatedString = 0x13,
        PascalString = 0x14,
        Group = 0x16
    }
}