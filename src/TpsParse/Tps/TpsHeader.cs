using System.IO;
using System.Text;
using TpsParse.Util;

namespace TpsParse.Tps
{
    /// <summary>
    ///     TpsHeader represents the first 0x200 bytes of a TPS file.
    ///     Aside from the 'tOpS' identifier, it holds an array of page addresses and other meta information.
    /// </summary>
    public class TpsHeader
    {
        public TpsHeader(Stream inputStream)
        {
            inputStream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[0x200];
            inputStream.Read(buffer, 0, 0x200);

            Addr = BitUtil.ToInt32(buffer, 0);
            HdrSize = BitUtil.ToInt16(buffer, 4);
            FileLength1 = BitUtil.ToInt32(buffer, 6);
            FileLength2 = BitUtil.ToInt32(buffer, 10);
            TopSpeed = Encoding.ASCII.GetString(buffer, 14, 4);
            Zeros = BitUtil.ToInt16(buffer, 18);
            LastIssuedRow = BitUtil.ToInt32(buffer, 20);
            Changes = BitUtil.ToInt32(buffer, 24);
            ManagementPageRef = BitUtil.ToInt32(buffer, 28);

            PageStart = GetInt32Array(buffer, 32, 60);
            PageEnd = GetInt32Array(buffer, 272, 60);
        }

        public int Addr { get; private set; }
        public short HdrSize { get; private set; }
        public int FileLength1 { get; private set; }
        public int FileLength2 { get; private set; }
        public string TopSpeed { get; }
        public short Zeros { get; private set; }
        public int LastIssuedRow { get; private set; }
        public int Changes { get; private set; }
        public int ManagementPageRef { get; private set; }
        public int[] PageStart { get; private set; }
        public int[] PageEnd { get; private set; }

        private static int[] GetInt32Array(byte[] buffer, int startIndex, int length)
        {
            var result = new int[length];
            for (var i = 0; i < length; i++)
            {
                result[i] = (BitUtil.ToInt32(buffer, startIndex + (i*4)) << 8) + 0x200;
            }
            return result;
        }

        public bool IsTopSpeedFile()
        {
            return TopSpeed == "tOpS";
        }
    }
}