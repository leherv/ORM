using System.Collections.Generic;
using ORM_Lib.DbSchema;

namespace ORM_Lib
{
    public class InternalDbSet<T> : DbSet<T> where T : class
    {
    }
}