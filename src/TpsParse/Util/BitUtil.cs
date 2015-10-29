using System.IO;
using System.Text;

namespace TpsParse.Util
{
    public static class BitUtil
    {
        public static int ToInt32(byte[] buffer, int offset)
        {
            return ToInt32(buffer, offset, true);
        }

        public static int ToInt32(byte[] buffer, int offset, bool littleEndian)
        {
            return !littleEndian
                ? (buffer[offset] << 24 | buffer[offset + 1] << 16 | buffer[offset + 2] << 8 | buffer[offset + 3] << 0)
                : (buffer[offset] << 0 | buffer[offset + 1] << 8 | buffer[offset + 2] << 16 | buffer[offset + 3] << 24);
        }

        public static uint ToUInt32(byte[] buffer, int offset)
        {
            return ToUInt32(buffer, offset, true);
        }

        public static uint ToUInt32(byte[] buffer, int offset, bool littleEndian)
        {
            return !littleEndian
                ? (uint)(buffer[offset] << 24 | buffer[offset + 1] << 16 | buffer[offset + 2] << 8 | buffer[offset + 3] << 0)
                : (uint)(buffer[offset] << 0 | buffer[offset + 1] << 8 | buffer[offset + 2] << 16 | buffer[offset + 3] << 24);
        }

        public static short ToInt16(byte[] buffer, int offset)
        {
            return ToInt16(buffer, offset, true);
        }

        public static short ToInt16(byte[] buffer, int offset, bool littleEndian)
        {
            return !littleEndian
                ? (short) (buffer[offset] << 8 | buffer[offset + 1] << 0)
                : (short) (buffer[offset] << 0 | buffer[offset + 1] << 8);
        }

        public static ushort ToUInt16(byte[] buffer, int offset)
        {
            return ToUInt16(buffer, offset, true);
        }

        public static ushort ToUInt16(byte[] buffer, int offset, bool littleEndian)
        {
            return !littleEndian
                ? (ushort)(buffer[offset] << 8 | buffer[offset + 1] << 0)
                : (ushort)(buffer[offset] << 0 | buffer[offset + 1] << 8);
        }

        public static short ToInt16(Stream stream)
        {
            return ToInt16(stream, true);
        }

        public static short ToInt16(Stream stream, bool littleEndian)
        {
            return ToInt16(new[] {(byte) stream.ReadByte(), (byte) stream.ReadByte()}, 0, littleEndian);
        }

        public static ushort ToUInt16(Stream stream)
        {
            return ToUInt16(stream, true);
        }

        public static ushort ToUInt16(Stream stream, bool littleEndian)
        {
            return ToUInt16(new[] { (byte)stream.ReadByte(), (byte)stream.ReadByte() }, 0, littleEndian);
        }

        public static int ToInt32(Stream stream)
        {
            return ToInt32(stream, true);
        }

        public static int ToInt32(Stream stream, bool littleEndian)
        {
            return
                ToInt32(
                    new[]
                    {
                        (byte) stream.ReadByte(), (byte) stream.ReadByte(), (byte) stream.ReadByte(),
                        (byte) stream.ReadByte()
                    }, 0, littleEndian);
        }

        public static uint ToUInt32(Stream stream)
        {
            return ToUInt32(stream, true);
        }

        public static uint ToUInt32(Stream stream, bool littleEndian)
        {
            return
                ToUInt32(
                    new[]
                    {
                        (byte) stream.ReadByte(), (byte) stream.ReadByte(), (byte) stream.ReadByte(),
                        (byte) stream.ReadByte()
                    }, 0, littleEndian);
        }

        public static string ZeroTerminatedString(Stream stream)
        {
            using (var str = new MemoryStream())
            {
                byte b;
                while ((b = (byte) stream.ReadByte()) != 0)
                {
                    str.WriteByte(b);
                }
                return Encoding.GetEncoding("iso-8859-1").GetString(str.ToArray());
            }
        }
    }
}