using CommandLine;

namespace TpsParse.Samples.Schema
{
    [Verb("schema", HelpText = "Print the schema of a TPS file")]
    public class Options
    {
        [Value(0, MetaName = "tpsfile", Required = true,
            HelpText = "TPS file to read from.")]
        public string InputFile { get; set; }
    }
}