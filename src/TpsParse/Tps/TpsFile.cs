using System;
using System.Collections.Generic;
using System.IO;
using TpsParse.Tps.Record;

namespace TpsParse.Tps
{
    public class TpsFile
    {
        private readonly Stream _inputStream;

        /// <summary>
        ///     Construct a new TpsFile from the given stream.
        /// </summary>
        /// <param name="inputStream">Stream containing the TPS file.</param>
        /// <remarks>The stream must be seekable.</remarks>
        public TpsFile(Stream inputStream)
        {
            if (!inputStream.CanSeek)
                throw new ArgumentException("The stream specified must be seekable.", nameof(inputStream));

            _inputStream = inputStream;
        }

        /// <summary>
        ///     Read the header of the TPS file.
        /// </summary>
        /// <returns>The TPS header.</returns>
        public TpsHeader GetHeader()
        {
            _inputStream.Seek(0, SeekOrigin.Begin);

            var header = new TpsHeader(_inputStream);
            if (!header.IsTopSpeedFile())
            {
                throw new NotATopSpeedFileException("Not a TopSpeed file (" + header.TopSpeed + ")");
            }
            return header;
        }

        public T Accept<T>(ITpsFileVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        /// <summary>
        ///     Get all the records in a file.
        /// </summary>
        /// <returns>All records in the file.</returns>
        public IEnumerable<TpsRecord> GetAllRecords()
        {
            foreach (var block in GetTpsBlocks())
            {
                foreach (var page in block.Pages)
                {
                    page.ParseRecords();
                    foreach (var record in page.Records)
                    {
                        yield return record;
                    }
                }
            }
        }

        /// <summary>
        ///     Get the blocks from the TPS file.
        /// </summary>
        /// <returns>Blocks from the TPS file.</returns>
        public List<TpsBlock> GetTpsBlocks()
        {
            var header = GetHeader();
            var results = new List<TpsBlock>();
            for (var t = 0; t < header.PageStart.Length; t++)
            {
                var ofs = header.PageStart[t];
                var end = header.PageEnd[t];

                // Skips the first entry (0 length) and any blocks that are beyond
                // the file size.
                if (!(ofs == 0x0200 && end == 0x200) && (ofs < _inputStream.Length))
                {
                    results.Add(new TpsBlock(_inputStream, ofs, end));
                }
            }
            return results;
        }

        /// <summary>
        ///     Get a list of table definitions in the file.
        /// </summary>
        /// <returns>A list of table definitions.</returns>
        /// <remarks>
        ///     Generally, most files will have a single table.
        /// </remarks>
        public List<TableDefinitionRecord> GetTableDefinitions()
        {
            return Accept(new TableDefinitionVisitor());
        }

        /// <summary>
        ///     Get a list of tables names in the file.
        /// </summary>
        /// <returns>A list of table names.</returns>
        public List<TableNameRecord> GetTableNames()
        {
            return Accept(new TableNameVisitor());
        }

        public List<DataRecord> GetDataRecords(TableDefinitionRecord tableDefinition)
        {
            return Accept(new DataRecordVisitor(tableDefinition));
        }

        private class TableDefinitionVisitor : AbstractTpsFileVisitor<List<TableDefinitionRecord>>
        {
            public override List<TableDefinitionRecord> Visit(TableDefinitionRecord record)
            {
                return new List<TableDefinitionRecord>(new[] {record});
            }

            public override List<TableDefinitionRecord> Visit(TpsFile file)
            {
                var tableMap = new Dictionary<int, List<TableDefinitionRecord>>();
                var records = new List<TableDefinitionRecord>();
                foreach (var record in file.GetAllRecords())
                {
                    var results = record.Accept(this);
                    if (results == null) continue;

                    foreach (var result in results)
                    {
                        List<TableDefinitionRecord> tableDefList;
                        if (!tableMap.TryGetValue(result.TableNumber, out tableDefList))
                        {
                            tableDefList = new List<TableDefinitionRecord>();
                            tableMap[result.TableNumber] = tableDefList;
                        }
                        tableDefList.Add(result);
                    }
                }
                foreach (var pair in tableMap)
                {
                    var definition = TableDefinitionRecord.Merge(pair.Value);
                    definition.ParseData();
                    records.Add(definition);
                }
                return records;
            }
        }

        private class TableNameVisitor : AbstractTpsFileVisitor<List<TableNameRecord>>
        {
            public override List<TableNameRecord> Visit(TableNameRecord record)
            {
                return new List<TableNameRecord>(new[] {record});
            }

            public override List<TableNameRecord> Visit(TpsFile file)
            {
                var records = new List<TableNameRecord>();
                foreach (var record in file.GetAllRecords())
                {
                    var result = record.Accept(this);
                    if (result != null)
                        records.AddRange(result);
                }
                return records;
            }
        }

        private class DataRecordVisitor : AbstractTpsFileVisitor<List<DataRecord>>
        {
            private readonly TableDefinitionRecord _tableDefinition;

            public DataRecordVisitor(TableDefinitionRecord tableDefinition)
            {
                _tableDefinition = tableDefinition;
            }

            public override List<DataRecord> Visit(DataRecord record)
            {
                if (record.TableNumber == _tableDefinition.TableNumber)
                {
                    record.ParseValues(_tableDefinition);
                    return new List<DataRecord>(new[] {record});
                }
                return null;
            }

            public override List<DataRecord> Visit(TpsFile file)
            {
                var records = new List<DataRecord>();
                foreach (var record in file.GetAllRecords())
                {
                    var result = record.Accept(this);
                    if (result != null)
                        records.AddRange(result);
                }
                return records;
            }
        }
    }
}