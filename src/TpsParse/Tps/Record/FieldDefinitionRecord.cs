using System;
using System.IO;
using TpsParse.Tps.Record.Fields;
using TpsParse.Util;

namespace TpsParse.Tps.Record
{
    public abstract class FieldDefinitionRecord
    {
        protected FieldDefinitionRecord(Stream inputStream)
        {
            Offset = BitUtil.ToInt16(inputStream);
            FieldName = BitUtil.ZeroTerminatedString(inputStream);
            Elements = BitUtil.ToInt16(inputStream);
            Length = BitUtil.ToInt16(inputStream);
            Flags = BitUtil.ToInt16(inputStream);
            Index = BitUtil.ToInt16(inputStream);
        }

        public short Offset { get; private set; }
        public string FieldName { get; private set; }
        public short Elements { get; }
        public short Length { get; private set; }
        public short Flags { get; private set; }
        public short Index { get; private set; }
        public FieldType Type { get; private set; }

        public static FieldDefinitionRecord GetField(Stream inputStream)
        {
            var type = (FieldType) inputStream.ReadByte();
            FieldDefinitionRecord field;

            switch (type)
            {
                case FieldType.Bcd:
                    field = new BcdField(inputStream);
                    break;

                case FieldType.Byte:
                    field = new ByteField(inputStream);
                    break;

                case FieldType.Date:
                    field = new DateField(inputStream);
                    break;

                case FieldType.Double:
                    field = new DoubleField(inputStream);
                    break;

                case FieldType.FixedLengthString:
                    field = new FixedLengthStringField(inputStream);
                    break;

                case FieldType.Float:
                    field = new FloatField(inputStream);
                    break;

                case FieldType.Group:
                    field = new GroupField(inputStream);
                    break;

                case FieldType.PascalString:
                    field = new PascalStringField(inputStream);
                    break;

                case FieldType.SignedLong:
                    field = new SignedLongField(inputStream);
                    break;

                case FieldType.SignedShort:
                    field = new SignedShortField(inputStream);
                    break;

                case FieldType.Time:
                    field = new TimeField(inputStream);
                    break;

                case FieldType.UnsignedLong:
                    field = new UnsignedLongField(inputStream);
                    break;

                case FieldType.UnsignedShort:
                    field = new UnsignedShortField(inputStream);
                    break;

                case FieldType.ZeroTerminatedString:
                    field = new ZeroTerminatedStringField(inputStream);
                    break;

                default:
                    throw new NotImplementedException(string.Format("The field type 0x{0:X2} is not implemented.", type));
            }

            field.Type = type;
            return field;
        }

        /// <summary>
        ///     checks if this field is a group field. Group fields
        ///     are overlays on top of existing fields and as such
        ///     may contain text or binary.
        /// </summary>
        /// <returns>true if this field is a group field</returns>
        public virtual bool IsGroup()
        {
            return false;
        }

        /// <summary>
        ///     Get the value of this field.
        /// </summary>
        /// <param name="inputStream">Stream for which to parse.</param>
        /// <returns>An object representing the value based on the type of the field.</returns>
        public abstract object GetValue(Stream inputStream);

        /// <summary>
        ///     Specifies whether or not this field contains an array.
        /// </summary>
        /// <returns>true if the field is an array</returns>
        public bool IsArray()
        {
            return Elements > 1;
        }

        /// <summary>
        ///     Gets the array value if applicable.
        /// </summary>
        /// <param name="inputStream">Stream from which to get the values.</param>
        /// <returns>An array of values.</returns>
        public object[] GetArrayValue(Stream inputStream)
        {
            var values = new object[Elements];
            for (var i = 0; i < Elements; i++)
            {
                values[i] = GetValue(inputStream);
            }
            return values;
        }
    }
}