using System;
using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record
{
    public class MemoDefinitionRecord
    {
        public MemoDefinitionRecord(Stream inputStream)
        {
            ExternalFile = BitUtil.ZeroTerminatedString(inputStream);
            if (ExternalFile.Length == 0)
            {
                if (inputStream.ReadByte() != 1)
                    throw new InvalidOperationException("Bad Memo Definition, missing 0x01 after zero string.");
            }
            Name = BitUtil.ZeroTerminatedString(inputStream);
            Length = BitUtil.ToInt16(inputStream);
            Flags = BitUtil.ToInt16(inputStream);
        }

        public string ExternalFile { get; }
        public string Name { get; private set; }
        public short Length { get; private set; }
        public short Flags { get; }

        public bool IsMemo()
        {
            return (Flags & 0x04) == 0;
        }

        public bool IsBlob()
        {
            return (Flags & 0x04) != 0;
        }
    }
}