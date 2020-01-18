using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Saving
{
    class PocoInsertChange
    {
        public string TableName { get; }
        public (string, object) colNameValue { get; }
        public (string, object) ColNameValue2 { get; }

        public PocoInsertChange(string tableName, (string, object) colNameValue, (string, object) colNameValue2)
        {
            TableName = tableName;
            this.colNameValue = colNameValue;
            ColNameValue2 = colNameValue2;
        }
    }
}
