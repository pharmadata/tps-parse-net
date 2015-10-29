using System.Collections.Generic;
using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps
{
    /// <summary>
    ///     The TpsPage holds TpsRecords, which may be compressed using Run Length Encoding (RLE).
    ///     There is no proper flag that signals if the page is compressed.
    ///     Currently the page is thought of as compressed when the pageSize is different from
    ///     the uncompressedSize<i> and</i> the flags are 0x00. Some records with the flag set
    ///     to a non zero value also have different lengths, but the data is not compressed..
    ///     Building the TpsRecords out of the TpsPage uses also a custom compression algorithm.
    ///     As most headers of the TpsRecords are identical, some bytes can be saved by reusing
    ///     the headers of previous records. The first byte of each TpsRecord indicates what
    ///     parts should be reused. Obviously the first record on a page should have a full
    ///     header (0xC0).
    /// </summary>
    public class TpsPage
    {
        private readonly byte[] _compressedData;
        private byte[] _data;

        public TpsPage(Stream inputStream)
        {
            var buffer = new byte[13];
            inputStream.Read(buffer, 0, 13);

            Addr = BitUtil.ToInt32(buffer, 0);
            PageSize = BitUtil.ToInt16(buffer, 4);
            PageSizeUncompressed = BitUtil.ToInt16(buffer, 6);
            PageSizeUncompressedWithoutHeader = BitUtil.ToInt16(buffer, 8);
            RecordCount = BitUtil.ToInt16(buffer, 10);
            Flags = buffer[12];

            _compressedData = new byte[PageSize - 13];
            inputStream.Read(_compressedData, 0, PageSize - 13);

            Records = new List<TpsRecord>();
        }

        public int Addr { get; private set; }
        public short PageSize { get; }
        public short PageSizeUncompressed { get; }
        public short PageSizeUncompressedWithoutHeader { get; private set; }
        public short RecordCount { get; }
        public byte Flags { get; }
        public List<TpsRecord> Records { get; }

        protected void Uncompress()
        {
            if (PageSize != PageSizeUncompressed && Flags == 0)
            {
                _data = RleDecoder.Decode(_compressedData);
            }
            else
            {
                _data = _compressedData;
            }
        }

        protected byte[] GetData()
        {
            if (IsFlushed())
            {
                Uncompress();
            }
            return _data;
        }

        protected bool IsFlushed()
        {
            return _data == null;
        }

        public void Flush()
        {
            _data = null;
            Records.Clear();
        }

        public void ParseRecords()
        {
            if (IsFlushed())
            {
                Uncompress();
            }
            Records.Clear();
            // Skip pages with non 0x00 flags as they don't seem to contain TpsRecords.
            if (Flags == 0x00)
            {
                using (var stream = new MemoryStream(_data))
                {
                    TpsRecord prev = null;
                    do
                    {
                        var current = prev == null ? TpsRecord.GetRecord(stream) : TpsRecord.GetRecord(stream, prev);
                        Records.Add(current);
                        prev = current;
                    } while (stream.Position != stream.Length - 1 && Records.Count < RecordCount);
                }
            }
        }
    }
}