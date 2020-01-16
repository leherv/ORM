using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Update
{
    class UpdateBatch : ISqlExpression
    {
        private List<UpdateStatement> _updateStatements;

        public UpdateBatch(List<UpdateStatement> updateStatements)
        {
            _updateStatements = updateStatements;
        }

        public string AsSqlString()
        {
            return _updateStatements
              .Select(up => $"{up.AsSqlString()}")
              .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}; {c2}");
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _updateStatements.SelectMany(updateS => updateS.GetNamedParams());
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {

        }
    }
}
