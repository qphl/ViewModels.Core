namespace CR.ViewModels.Tests
{
    public class TestEntity2
    {
        public TestEntity2(string id, int field1)
        {
            Identifier = id;
            Field1 = field1;
        }

        public string Identifier { get; set; }
        public int Field1 { get; set; }

        protected bool Equals(TestEntity2 other)
        {
            return string.Equals(Identifier, other.Identifier) && Field1 == other.Field1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TestEntity2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Identifier != null ? Identifier.GetHashCode() : 0)*397) ^ Field1;
            }
        }
    }
}