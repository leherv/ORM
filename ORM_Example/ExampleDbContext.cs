using System.Buffers.Text;
using System.Data;
using System.Runtime.CompilerServices;
using ORM_Lib;
using ORM_Lib.TypeMapper;

namespace ORM_Example
{
    public class ExampleDbContext : DbContext
    {
        public override IDbConnection DbConnection { get; }
        protected override ITypeMapper TypeMapper => new PostgresTypeMapper();
        public DbSet<Person> Persons { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

    }
}