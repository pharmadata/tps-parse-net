using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using TpsParse.Tps;

namespace TpsParse.Samples.Csv
{
    public static class ExportToCsv
    {
        public static int Export(Options options)
        {
            FileStream file;
            try
            {
                file = File.Open(options.InputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File '{0}' was not found.", options.InputFile);
                return 1;
            }
            catch (IOException)
            {
                Console.WriteLine("Unable to open file '{0}'.", options.InputFile);
                return 1;
            }
            
            try
            {
                var tps = new TpsFile(file);

                var definitions = tps.GetTableDefinitions();
                if (definitions.Count > 1)
                {
                    Console.WriteLine("Unable to export TPS files with multiple tables to CSV.");
                    return 1;
                }

                using (var writer = File.CreateText(options.OutputFile))
                using (var csv = new CsvWriter(writer))
                {
                    foreach (var field in definitions[0].Fields)
                    {
                        var name = field.FieldName.Substring(field.FieldName.IndexOf(":") + 1);

                        if (field.IsArray())
                        {
                            for (var i = 0; i < field.Elements; i++)
                            {
                                csv.WriteField(name + "[" + i + "]");
                            }
                            continue;
                        }

                        csv.WriteField(name);
                    }
                    csv.NextRecord();

                    foreach (var record in tps.GetDataRecords(definitions[0]))
                    {
                        foreach (var value in record.Values)
                        {
                            var array = value as IEnumerable<object>;
                            if (array == null)
                                csv.WriteField(value);
                            else
                            {
                                foreach (var arrayValue in array)
                                    csv.WriteField(arrayValue);
                            }
                        }
                        csv.NextRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex);
                return 1;
            }
            finally
            {
                file.Dispose();
            }

            return 0;
        }
    }
}