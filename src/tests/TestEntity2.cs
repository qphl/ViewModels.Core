namespace CR.ViewModels.Tests
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

        protected bool Equals(TestEntity2 other)
        {
            return string.Equals(Id, other.Id) && Field1 == other.Field1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestEntity2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id != null ? Id.GetHashCode() : 0)*397) ^ Field1;
            }
        }
    }
}