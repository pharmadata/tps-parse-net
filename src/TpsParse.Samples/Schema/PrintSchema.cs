using System;
using System.IO;
using System.Linq;
using TpsParse.Tps;

namespace TpsParse.Samples.Schema
{
    public static class PrintSchema
    {
        public static int Print(Options options)
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
                var map = tps.GetTableNames().ToDictionary(n => n.TableNumber, n => new
                {
                    n.Name,
                    Definition = definitions.First(d => d.TableNumber == n.TableNumber)
                });

                foreach (var table in map)
                {
                    Console.WriteLine("Table ID: {0} ({1})", table.Key, table.Value.Name);
                    Console.WriteLine();

                    foreach (var field in table.Value.Definition.Fields)
                    {
                        Console.WriteLine("{0} ({1})", field.FieldName, field.Type);
                    }
                }
            }
            finally
            {
                file.Dispose();
            }

            return 0;
        }
    }
}