namespace cr.viewmodels.tests
{
    public class TestEntity1
    {
        public TestEntity1(string id, string field1)
        {
            Id = id;
            Field1 = field1;
        }

        public string Id { get; set; }
        public string Field1 { get; set; }
    }
}