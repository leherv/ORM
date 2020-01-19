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
        private (string,string) _colNames;
        
        public ManyToManyInsertStatementBuilder(string tableName, List<PocoInsertChange> insertChanges)
        {
            _tableName = tableName;
            _insertChanges = insertChanges;
            _colNames = BuildColumnNames();
        }

        private (string, string) BuildColumnNames()
        {
            var insertChange = _insertChanges.First();
            return (insertChange.colNameValue.Item1, insertChange.ColNameValue2.Item1);
        }

        // probably will have to be Hashset<long, long> so comparing will work
        private HashSet<(object, object)> BuildValues()
        {
            var set = new HashSet<(object, object)>();
            foreach(var change in _insertChanges)
            {
                if(change.colNameValue.Item1.Equals(_colNames.Item1))
                {
                    set.Add((change.colNameValue.Item2, change.ColNameValue2.Item2));
                } else
                {
                    set.Add((change.ColNameValue2.Item2, change.colNameValue.Item2));
                }
            }
            return set;
        }

        public ManyToManyInsertStatement Build()
        {
            return new ManyToManyInsertStatement(
                _tableName,
                _colNames,
                BuildValues()
            );
        }


    }
}
