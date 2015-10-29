using System.Collections.Generic;
using System.IO;
using TpsParse.Util;

namespace TpsParse.Tps.Record
{
    public class DataRecord : TpsRecord
    {
        public DataRecord(byte[] data, byte[] header)
            : base(data, header, true)
        {
            RecordNumber = BitUtil.ToInt32(header, 5, false);
        }

        public int RecordNumber { get; private set; }
        public List<object> Values { get; private set; }

        /// <summary>
        ///     To parse the values in a field, you must pass the table definition.
        /// </summary>
        /// <param name="record">Table definition of the data field.</param>
        public void ParseValues(TableDefinitionRecord record)
        {
            var values = new List<object>(record.Fields.Count);
            using (var stream = new MemoryStream(Data))
            {
                foreach (var field in record.Fields)
                {
                    values.Add(field.IsArray() ? field.GetArrayValue(stream) : field.GetValue(stream));
                }
            }
            Values = values;
        }

        public override T Accept<T>(ITpsFileVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}