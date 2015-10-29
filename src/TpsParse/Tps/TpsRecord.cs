using System;
using System.IO;
using TpsParse.Tps.Record;
using TpsParse.Util;

namespace TpsParse.Tps
{
    public abstract class TpsRecord
    {
        protected TpsRecord(byte[] data, byte[] header, bool readTable)
        {
            Data = data;
            Header = header;

            if (readTable)
            {
                TableNumber = BitUtil.ToInt32(header, 0, false);
            }
        }

        protected byte[] Header { get; }
        protected byte[] Data { get; }
        public int TableNumber { get; protected set; }

        public static TpsRecord GetRecord(Stream inputStream, TpsRecord previous)
        {
            var buffer = new byte[2];
            var flags = (byte) inputStream.ReadByte();

            short recordLength;
            short headerLength;

            if ((flags & 0x80) != 0)
            {
                inputStream.Read(buffer, 0, 2);
                recordLength = BitUtil.ToInt16(buffer, 0);
            }
            else
            {
                recordLength = (short) (previous.Data.Length + previous.Header.Length);
            }
            if ((flags & 0x40) != 0)
            {
                inputStream.Read(buffer, 0, 2);
                headerLength = BitUtil.ToInt16(buffer, 0);
            }
            else
            {
                headerLength = (short) previous.Header.Length;
            }
            var copy = flags & 0x3F;
            byte[] allData;
            try
            {
                allData = new byte[recordLength];
                if (copy > 0)
                {
                    Array.Copy(previous.Header, 0, allData, 0,
                        copy > previous.Header.Length ? previous.Header.Length : copy);
                    if (copy > previous.Header.Length)
                        Array.Copy(previous.Data, 0, allData, previous.Header.Length, copy - previous.Header.Length);
                }
                inputStream.Read(allData, copy, allData.Length - copy);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "When reading " + (recordLength - copy) + " bytes of TpsRecord at " +
                    inputStream.Position, ex);
            }

            var header = new byte[headerLength];
            var data = new byte[recordLength - headerLength];

            Array.Copy(allData, 0, header, 0, headerLength);
            Array.Copy(allData, headerLength, data, 0, recordLength - headerLength);

            return CreateRecord(header, data);
        }

        public static TpsRecord GetRecord(Stream inputStream)
        {
            var buffer = new byte[5];
            inputStream.Read(buffer, 0, 5);

            var flags = buffer[0];
            if ((flags & 0xC0) != 0xC0)
                throw new InvalidOperationException(
                    string.Format("Can't construct a TpsRecord without record lengths (0x{0:X2})", flags));

            var recordLength = BitUtil.ToInt16(buffer, 1);
            var headerLength = BitUtil.ToInt16(buffer, 3);

            var header = new byte[headerLength];
            inputStream.Read(header, 0, headerLength);

            var data = new byte[recordLength - headerLength];
            inputStream.Read(data, 0, recordLength - headerLength);

            return CreateRecord(header, data);
        }

        private static TpsRecord CreateRecord(byte[] header, byte[] data)
        {
            if (header.Length < 5)
                return new EmptyRecord(data, header);

            if (header[0] == 0xFE)
                return new TableNameRecord(data, header);

            switch (header[4])
            {
                case 0xF3:
                    return new DataRecord(data, header);
                case 0xF6:
                    return new MetadataRecord(data, header);
                case 0xFA:
                    return new TableDefinitionRecord(data, header);
                case 0xFC:
                    return new MemoRecord(data, header);
                default:
                    return new IndexRecord(data, header);
            }
        }

        public abstract T Accept<T>(ITpsFileVisitor<T> visitor);
    }
}