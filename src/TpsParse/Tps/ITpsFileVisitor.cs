using TpsParse.Tps.Record;

namespace TpsParse.Tps
{
    public interface ITpsFileVisitor<out T>
    {
        T Visit(DataRecord record);
        T Visit(EmptyRecord record);
        T Visit(IndexRecord record);
        T Visit(MemoRecord record);
        T Visit(MetadataRecord record);
        T Visit(TableDefinitionRecord record);
        T Visit(TableNameRecord record);
        T Visit(TpsFile file);
    }
}