using ORM_Example;
using ORM_Lib;

namespace ORM_Tests
{
    public class ExampleDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}