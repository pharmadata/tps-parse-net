using TpsParse.Tps.Record;

namespace TpsParse.Tps
{
    public abstract class AbstractTpsFileVisitor<T> : ITpsFileVisitor<T>
    {
        public virtual T Visit(DataRecord record)
        {
            return default(T);
        }

        public virtual T Visit(EmptyRecord record)
        {
            return default(T);
        }

        public virtual T Visit(IndexRecord record)
        {
            return default(T);
        }

        public virtual T Visit(MemoRecord record)
        {
            return default(T);
        }

        public virtual T Visit(MetadataRecord record)
        {
            return default(T);
        }

        public virtual T Visit(TableDefinitionRecord record)
        {
            return default(T);
        }

        public virtual T Visit(TableNameRecord record)
        {
            return default(T);
        }

        public virtual T Visit(TpsFile file)
        {
            return default(T);
        }
    }
}