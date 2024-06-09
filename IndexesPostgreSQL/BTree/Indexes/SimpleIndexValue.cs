using System;

namespace IndexesPostgreSQL
{
    public class SimpleIndexValue : IComparable<SimpleIndexValue>
    {
        public int? Value { get; set; }

        public SimpleIndexValue(int? value)
        {
            Value = value;
        }

        public int CompareTo(SimpleIndexValue other)
        {
            if (other == null) return 1; // Consider null less than any instance
            return Nullable.Compare(this.Value, other.Value);
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "null";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (SimpleIndexValue)obj;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}
