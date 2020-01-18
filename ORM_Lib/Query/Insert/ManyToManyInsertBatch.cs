using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Insert
{
    class ManyToManyInsertBatch: ISqlExpression
    {
        private List<ManyToManyInsertStatement> _insertStatements;

        public ManyToManyInsertBatch(List<ManyToManyInsertStatement> insertStatements)
        {
            _insertStatements = insertStatements;
        }

        public string AsSqlString()
        {
            return _insertStatements
              .Select(up => $"{up.AsSqlString()}")
              .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}; {c2}");
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _insertStatements.SelectMany(insertS => insertS.GetNamedParams());
        }

    
        void ISqlExpression.SetContextInformation(Entity entity)
        {

        }

    }
}
