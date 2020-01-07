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

        public List<Type> SuperClasses { get; set; }

        public string Alias { get; set; }

        public Entity(string eName, List<Column> columns, Type pocoType, Column pkColumn, List<Type> superClasses, int aliasNumber)
        {
            Name = eName;
            Columns = columns;
            PocoType = pocoType;
            PkColumn = pkColumn;
            SuperClasses = superClasses;
            Alias = BuildAlias(aliasNumber);
        }

        public Column GetColumnByName(string name)
        {
            return Columns.SingleOrDefault(c => c.Name == name);
        }

        private string BuildAlias(int i)
        {
            return $"{Name.First()}{i}";
        }

    }
}