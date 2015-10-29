using System.IO;

namespace TpsParse.Tps
{
    /// <summary>
    ///     Handles the RLE (run length encoding) decoding.
    /// </summary>
    public static class RleDecoder
    {
        public static byte[] Decode(byte[] data)
        {
            using (var output = new MemoryStream())
            {
                try
                {
                    var pos = 0;
                    do
                    {
                        int skip = data[pos++];
                        if (skip == 0)
                        {
                            throw new RunLengthEncodingException("Bad RLE Skip (0x00)");
                        }
                        if (skip > 0x7F)
                        {
                            var msb = data[pos++];
                            var lsb = (skip & 0x7F);
                            var shift = 0x80*(msb & 0x01);
                            skip = ((msb << 7) & 0x00FF00) + lsb + shift;
                        }
                        output.Write(data, pos, skip);
                        pos += skip;

                        if (pos >= data.Length)
                            break;

                        var toRepeat = data[pos - 1];
                        int repeatsMinusOne = data[pos++];
                        if (repeatsMinusOne > 0x7F)
                        {
                            var msb = data[pos++];
                            var lsb = (repeatsMinusOne & 0x7F);
                            var shift = 0x80*(msb & 0x01);
                            repeatsMinusOne = ((msb << 7) & 0x00FF00) + lsb + shift;
                        }
                        for (var t = 0; t < repeatsMinusOne; t++)
                            output.WriteByte(toRepeat);
                    } while (pos < data.Length - 1);
                }
                catch (IOException ex)
                {
                    throw new RunLengthEncodingException("Error while RLE decoding.", ex);
                }
                return output.ToArray();
            }
        }
    }
}