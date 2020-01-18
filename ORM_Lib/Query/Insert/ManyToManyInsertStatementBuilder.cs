using ORM_Lib.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Insert
{
    class ManyToManyInsertStatementBuilder
    {
        private string _tableName;
        private List<PocoInsertChange> _insertChanges;
        
        public ManyToManyInsertStatementBuilder(string tableName, List<PocoInsertChange> insertChanges)
        {
            _tableName = tableName;
            _insertChanges = insertChanges;
        }

        private (string, string) BuildColumnNames()
        {
            var insertChange = _insertChanges.First();
            return (insertChange.colNameValue.Item1, insertChange.ColNameValue2.Item1);
        }

        // probably will have to be Hashset<long, long> so comparing will work
        private HashSet<(object, object)> BuildValues()
        {
            return _insertChanges
                .Select(i => ((i.colNameValue.Item2, i.ColNameValue2.Item2)))
                .ToHashSet();
        }

        public ManyToManyInsertStatement Build()
        {
            return new ManyToManyInsertStatement(
                _tableName,
                BuildColumnNames(),
                BuildValues()
            );
        }


    }
}
