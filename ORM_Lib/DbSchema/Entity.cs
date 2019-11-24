using System;
using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.DbSchema
{
    internal class Entity
    {
        
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        
        public Column PkColumn { get; set; }
        
        public Type PocoType { get; set; } 


        public Entity(string eName, List<Column> columns, Type pocoType, Column pkColumn)
        {
            Name = eName;
            Columns = columns;
            PocoType = pocoType;
            PkColumn = pkColumn;
        }

        public Column GetColumnByName(string name)
        {
            return Columns.Single(c => c.Name == name);
        }

    }
}