using Npgsql;
using ORM_Lib;
using ORM_Lib.TypeMapper;

namespace ORM_Example
{
    public class ExampleDbContext : DbContext
    {
      
        protected override ORMConfiguration Configuration => new ORMConfiguration(
            new PostgresTypeMapper(),
            () => new NpgsqlConnection(ConnectionStringBuilder.connectionString()),
            true
        );

        public DbSet<Person> Persons { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

    }
}