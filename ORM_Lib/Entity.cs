using System.Collections.Generic;

namespace ORM_Lib
{
    public class Entity
    {
        
        public string Name { get; set; }
        public List<Column> Columns { get; set; }


        public Entity(string eName, List<Column> columns)
        {
            Name = eName;
            Columns = columns;
        }
        
    }
}