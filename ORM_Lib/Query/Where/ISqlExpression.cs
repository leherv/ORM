using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Query.Where
{
    public interface ISqlExpression
    {

        string AsSqlString();

        IEnumerable<NamedParameter> GetNamedParams();



    }
}
