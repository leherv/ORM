using System.Collections.Generic;

namespace ORM_Lib
{
    public class InternalDbSet<T> : DbSet<T> where T : class
    {
        
        public IEnumerable<Column> Columns { get; set; }
    }
}