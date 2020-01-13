# ORM

This is a university project. Do not use in production! I mean it. Don`t.
This project is a lazy-loading Object-Relational-Mapper that uses Code-First. Inheritance is handled with table-per-type.
Object Identity is ensured via Cache.
In the Usage description below I will always relate to the example project ORM_Example that can be found in this repository._

## Usage

### Context
First you have to define a DbContext. Your DbContext HAS to extend the DbContext class.

The ORMConfiguration consists of 
* a ITypeMapper that you can provide or you can use the one provided (only for Postgres DBs)
* a connectionstring - the example uses a ConnectionStringBuilder
* a boolean dictating if the database should be created (Code First) 

For every class you want to add to the database and later want to interact with you have to create a DBSet.

```
 public class ExampleDbContext : DbContext
    {
      
        protected override ORMConfiguration Configuration => new ORMConfiguration(
            new PostgresTypeMapper(),
            ConnectionStringBuilder.connectionString(),
            false
        );

        public DbSet<Person> Persons { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }


    }

```

### POCOs
As mentioned you have to create DbSets for all the future Tables.
Every DbSet corresponds to a POCO in your program and has to follow some rules.


#### Basics

Let us look at an example:
```
public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; } 
        public Gender Gender { get; set; }
        public DateTime BDay { get; set; }

        public Person(string name, string firstName, Gender gender, DateTime bDay)
        {
            Name = name;
            FirstName = firstName;
            Gender = gender;
            BDay = bDay;
        }
        public Person() { }
    }
```

```
    [TableName("Custom_name")]
    public class Teacher : Person
    {
        private ILazyLoader LazyLoader { get; set; } = new LazyLoader();

        [ColumnName("Custom_name")]
        public int Salary { get; set; }

        [Ignore]
        public string FavoriteQuote { get; set; }

        [OneToMany]
        public ICollection<Course> Courses
        {
            get => LazyLoader.Load(this, ref _courses);
            set => _courses = value;
        }
        private ICollection<Course> _courses;

        [OneToMany]
        public ICollection<Class> Classes
        {
            get => LazyLoader.Load(this, ref _classes);
            set => _classes = value;
        }
        private ICollection<Class> _classes;

        public Teacher(string name, string firstName, Gender gender, DateTime bDay, int salary, List<Course> courses) : base(name, firstName, gender, bDay)
        {
            Salary = salary;
            Courses = courses;
        }

        public Teacher() { }
    }
```
* If you do not define a table-name with **[TableName("")]** the lowercase name of the class is used as a name.
* If you do not define a column-name with **[ColumnName("")]** the lowercase name of the property is used as a name.
* Every POCO has to have a **Primary Key**
  * a property with the attribute **[Pk]** is found
  * a property named "id" (case insensitive) is found
  * if none of the above are the case an exception is thrown 
* If you want to exclude properties/fields you can use the **[Ignore]** property
* Every POCO **HAS to have a default constructor**.
* Every column that should be passivated **HAS to have a public getter and setter**


#### Relations

```
    public class Course
    {
        private ILazyLoader LazyLoader { get; set; } = new LazyLoader();

        [Pk]
        public long OtherNameThanId { get; set; }
        public string Name { get; set; }

        public bool Active { get; set; }


        [ManyToOne]
        public Teacher Teacher
        {
            get => LazyLoader.Load(this, ref _teacher);
            set => _teacher = value;
        }
        private Teacher _teacher;

        [ManyToMany(TableName = "student_course", ForeignKeyNear = "fk_person_id", ForeignKeyFar = "fk_course_id")]
        public ICollection<Student> Students
        {
            get => LazyLoader.Load(this, ref _students);
            set => _students = value;
        }
        private ICollection<Student> _students;

        public Course(bool active, Teacher teacher, List<Student> students)
        {
            Active = active;
            Teacher = teacher;
            Students = students;
        }

        public Course() { }
    }

```
The Following Relations are supported:
* **1:1 --> [Fk]**
* **n:n --> [ManyToMany(TableName="", ForeignKeyNear="", ForeignKeyFar="")]**
* **1:n --> [OneToMany(mappedByColumnName="")]**
* **n:1 --> [ManyToOne]**

All those relations are lazily loaded. For this to succeed you have to add:

```
 private ILazyLoader LazyLoader { get; set; } = new LazyLoader();
```
to every class that uses at least one of those attributes.

Every Lazy-Loading-Property has to be formed like this:

For Collections:
```
        [OneToMany(mappedByColumnName ="teacher")]
        public ICollection<Course> Courses
        {
            get => LazyLoader.Load(this, ref _courses);
            set => _courses = value;
        }
        private ICollection<Course> _courses;
```
**Note:** ICollection has to be used.

For single objects:
```
        [ManyToOne]
        public Teacher Teacher
        {
            get => LazyLoader.Load(this, ref _teacher);
            set => _teacher = value;
        }
        private Teacher _teacher;
```


#### Querying

##### Simple Select
Select can be executed on the DbSets to fetch entities corresponding to the type of the DbSet.
You can **query all columns** like this:
```
    var persons = dbContext.Persons.Select(new string[] { }).Build().Execute();
    var persons = dbContext.Persons.Select(null).Build().Execute();
```

One can also execute **partial selects** with only some columns which then have to be passed with the column-name as string.
```
    var partialPersons = dbContext.Persons.Select(new[] { "name", "firstname" }).Build().Execute();
```

##### Select with Where-Filter
You can also filter entities to select at database-level like this:

```
    var filteredPerson = dbContext.Persons
        .Select(null)
        .Where(BinaryExpression.Eq(new ColumnExpression("firstname"), new ValueExpression("test_firstname")))
        .Build()
        .Execute();
```
* There are a couple of predefined BinaryExpressions, all they do is automatically set the operator. You can also create a BinaryExpression with another operator by using the normal constructor.
* ColumnExpression defines which column will be queried
* ValueExpression sets the value for the comparison

You can also use two ValueExpressions (although you could just drop the whole Where) like this:
```
    var filteredPerson = dbContext.Persons
        .Select(null)
        .Where(BinaryExpression.Eq(new ValueExpression(1), new ValueExpression(1)))
        .Build()
        .Execute();
```


#### Adding
Inserting objects into the database works like this:

##### Single Object
```
    dbContext.Persons
        .Add(new Person("test_name", "test_firstname", Gender.MALE, new DateTime(2000,1,1)))
        .Build()
        .Execute();

```

##### Multiple Objects
```
    dbContext.Persons
        .Add(new[] {
            new Person("test_name2", "test_firstname2", Gender.FEMALE, new DateTime(2002,2,2)),
            new Person("test_name3", "test_firstname3", Gender.MALE, new DateTime(2003,3,3))
        })
        .Build()
        .Execute();
```

#### Saving





#### Important Mentions

```
    var persons = dbContext.Persons.Select(null).Build().Execute();
    // firstName from database = "john"
    var person = persons.First();
    person.FirstName = "Tom";

    var persons2 = dbContext.Persons.Select(null).Build().Execute();
    // firstName = "john" as the change was not saved!
    var firstName = person.FirstName;
```

* Inheritance is supported but only 1 level (Example-Project: Teacher - Person)



