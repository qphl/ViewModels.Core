namespace CR.ViewModels.Tests
{
    public class TestEntity1
    {
        public TestEntity1(string id, string field1)
        {
            Identifier = id;
            Field1 = field1;
        }

        public string Identifier { get; set; }
        public string Field1 { get; set; }

        protected bool Equals(TestEntity1 other)
        {
            return string.Equals(Identifier, other.Identifier) && string.Equals(Field1, other.Field1);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TestEntity1) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Identifier != null ? Identifier.GetHashCode() : 0)*397) ^ (Field1 != null ? Field1.GetHashCode() : 0);
            }
        }
    }
}