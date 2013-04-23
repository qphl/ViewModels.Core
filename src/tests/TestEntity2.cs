namespace cr.viewmodels.tests
{
    public class TestEntity2
    {
        public TestEntity2(string id, int field1)
        {
            Id = id;
            Field1 = field1;
        }

        public string Id { get; set; }
        public int Field1 { get; set; }
    }
}