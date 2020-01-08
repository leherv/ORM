using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ORM_Lib.TypeMapper
{
    class OrmDbType
    {

        public DbType PStmtDbType { get; }
        public string DdlDbType { get; }

        public OrmDbType(string ddlDbType, DbType pStmtDbType)
        {
            PStmtDbType = pStmtDbType;
            DdlDbType = ddlDbType;
        }
    }
}
