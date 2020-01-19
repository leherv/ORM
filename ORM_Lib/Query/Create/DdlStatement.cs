using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Create
{
    class DdlStatement : ISqlExpression
    {
        private string _ddlString;

        public DdlStatement(string ddlString)
        {
            _ddlString = ddlString;
        }

        public string AsSqlString()
        {
            return _ddlString;
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return Enumerable.Empty<NamedParameter>();
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {
           
        }
    }
}
