using CommandLine;

namespace TpsParse.Samples.Csv
{
    [Verb("csv", HelpText = "Export a TPS file to CSV")]
    public class Options
    {
        [Value(0, MetaName = "tpsfile", Required = true,
            HelpText = "TPS file to read from.")]
        public string InputFile { get; set; }
        
        [Value(1, MetaName = "csvfile", Required = true,
            HelpText = "CSV file to write output to.")]
        public string OutputFile { get; set; }
    }
}