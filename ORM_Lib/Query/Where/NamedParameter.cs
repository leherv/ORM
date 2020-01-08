using System.Data;

namespace ORM_Lib.Query.Where
{
    public class NamedParameter
    {
        public DbType DbType { get; }
        public string Alias { get; }
        public object Value { get; }

        public NamedParameter(string alias, object value, DbType dbType)
        {
            Alias = alias;
            DbType = dbType;
            Value = value;
        }

    }
}
