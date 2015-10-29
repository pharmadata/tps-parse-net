using System;
using System.Collections.Generic;
using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record
{
    public class IndexDefinitionRecord
    {
        public IndexDefinitionRecord(Stream inputStream)
        {
            ExternalFile = BitUtil.ZeroTerminatedString(inputStream);
            if (ExternalFile.Length == 0)
            {
                if (inputStream.ReadByte() != 1)
                    throw new InvalidOperationException("Bad Index Definition, missing 0x01 after zero string.");
            }
            Name = BitUtil.ZeroTerminatedString(inputStream);
            Flags = (byte) inputStream.ReadByte();
            FieldsInKey = BitUtil.ToInt16(inputStream);

            KeyField = new short[FieldsInKey];
            KeyFieldFlag = new short[FieldsInKey];

            for (var i = 0; i < FieldsInKey; i++)
            {
                KeyField[i] = BitUtil.ToInt16(inputStream);
                KeyFieldFlag[i] = BitUtil.ToInt16(inputStream);
            }
        }

        public string ExternalFile { get; }
        public string Name { get; private set; }
        public byte Flags { get; private set; }
        public short FieldsInKey { get; }
        public short[] KeyField { get; }
        public short[] KeyFieldFlag { get; }

        public List<FieldDefinitionRecord> GetFieldRecords(TableDefinitionRecord record)
        {
            var results = new List<FieldDefinitionRecord>();
            for (var i = 0; i < KeyField.Length; i++)
            {
                results.Add(record.Fields[i]);
            }
            return results;
        }
    }
}