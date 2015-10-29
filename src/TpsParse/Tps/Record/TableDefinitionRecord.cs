using System;
using System.Collections.Generic;
using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record
{
    public class TableDefinitionRecord : TpsRecord
    {
        private readonly byte[] _data;

        public TableDefinitionRecord(byte[] data, byte[] header)
            : base(data, header, true)
        {
            Block = BitUtil.ToInt16(header, 5, false);
            _data = data;
        }

        public int Block { get; private set; }
        public short DriverVersion { get; private set; }
        public short RecordLength { get; private set; }
        public short NumberOfFields { get; private set; }
        public short NumberOfMemos { get; private set; }
        public short NumberOfIndexes { get; private set; }
        public List<FieldDefinitionRecord> Fields { get; private set; }
        public List<MemoDefinitionRecord> Memos { get; private set; }
        public List<IndexDefinitionRecord> Indexes { get; private set; }

        /// <summary>
        ///     This should only be called once table definitions are merged.
        /// </summary>
        public void ParseData()
        {
            using (var stream = new MemoryStream(_data))
            {
                DriverVersion = BitUtil.ToInt16(stream);
                RecordLength = BitUtil.ToInt16(stream);
                NumberOfFields = BitUtil.ToInt16(stream);
                NumberOfMemos = BitUtil.ToInt16(stream);
                NumberOfIndexes = BitUtil.ToInt16(stream);

                Fields = new List<FieldDefinitionRecord>();
                Memos = new List<MemoDefinitionRecord>();
                Indexes = new List<IndexDefinitionRecord>();

                try
                {
                    for (var t = 0; t < NumberOfFields; t++)
                    {
                        Fields.Add(FieldDefinitionRecord.GetField(stream));
                    }
                    for (var t = 0; t < NumberOfMemos; t++)
                    {
                        Memos.Add(new MemoDefinitionRecord(stream));
                    }
                    for (var t = 0; t < NumberOfIndexes; t++)
                    {
                        Indexes.Add(new IndexDefinitionRecord(stream));
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Bad Table Definition", ex);
                }
            }
        }

        public override T Accept<T>(ITpsFileVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public static TableDefinitionRecord Merge(List<TableDefinitionRecord> records)
        {
            using (var data = new MemoryStream())
            {
                foreach (var record in records)
                {
                    data.Write(record.Data, 0, record.Data.Length);
                }
                return new TableDefinitionRecord(data.ToArray(), records[0].Header);
            }
        }
    }
}