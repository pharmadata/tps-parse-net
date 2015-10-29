using CommandLine;
using TpsParse.Samples.Csv;
using TpsParse.Samples.Schema;
using Options = TpsParse.Samples.Csv.Options;

namespace TpsParse.Samples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options, Schema.Options>(args)
                .MapResult(
                    (Options opts) => ExportToCsv.Export(opts),
                    (Schema.Options opts) => PrintSchema.Print(opts),
                    errs => 1);
        }
    }
}