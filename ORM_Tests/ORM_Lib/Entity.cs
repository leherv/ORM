namespace ORM_Tests.ORM_Lib
{
    public class TestEntity
    {

        public long Id { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }

        public TestEntity(string column1, string column2)
        {
            Column1 = column1;
            Column2 = column2;
        }

        public TestEntity() { }
    }
}