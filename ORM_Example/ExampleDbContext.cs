using ORM;
using ORM_Lib;

namespace ORM_Tests
{
    public class ExampleDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Class> Classes { get; set; }
    }
}