using System.Collections.Generic;
using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps
{
    /// <summary>
    ///     A TpsBlock is the outermost container for data, it groups a number of TpsPages.
    ///     As far as I know a TpsBlock holds no information about the TpsPages it contains.
    ///     Currently the pages are identified by scanning for them.
    ///     The current algorithm works by beginning at the block start,
    ///     parsing the TpsPage and then seeking for the next TpsPage using its
    ///     offset in the file(always at a 0x0100 boundary and the value at that
    ///     address must have the same value as the offset). Far from perfect but
    ///     it seems to work.
    /// </summary>
    public class TpsBlock
    {
        public TpsBlock(Stream inputStream, int start, int end)
        {
            Pages = new List<TpsPage>();

            inputStream.Seek(start, SeekOrigin.Begin);

            while (inputStream.Position < end)
            {
                var page = new TpsPage(inputStream);
                Pages.Add(page);

                if ((inputStream.Position & 0xFF) == 0)
                {
                    // Actually we might be already at a new page.
                }
                else
                {
                    // Jump to the next probable page.
                    inputStream.Seek((inputStream.Position & 0xFFFFFF00) + 0x0100, SeekOrigin.Begin);
                }
                var addr = 0;
                while ((addr != inputStream.Position) && inputStream.Position < inputStream.Length - 1)
                {
                    addr = BitUtil.ToInt32(inputStream);
                    inputStream.Seek(-4, SeekOrigin.Current);

                    // check if there is really a page here.
                    // if so, the offset in the file must match the value.
                    // if not, we continue.
                    if (addr != inputStream.Position)
                    {
                        inputStream.Seek(0x1000 - 4, SeekOrigin.Current);
                    }
                }
            }
        }

        public List<TpsPage> Pages { get; }

        public void Flush()
        {
            foreach (var page in Pages)
            {
                page.Flush();
            }
        }
    }
}