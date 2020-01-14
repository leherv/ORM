using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Query
{
    public interface ISqlExpression
    {

        string AsSqlString();

        IEnumerable<NamedParameter> GetNamedParams();

        internal void SetContextInformation(Entity entity);


    }
}
