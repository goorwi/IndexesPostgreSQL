using System;

namespace IndexesPostgreSQL
{
    public class IncludedIndexValue : IComparable<IncludedIndexValue>
    {
        public int? Value { get; set; }

        public int? firstField { get; set; }
        public int? secondField { get; set; }

        public IncludedIndexValue(int? value, int? firstField, int? secondField)
        {
            Value = value;
            this.firstField = firstField;
            this.secondField = secondField;
        }

        public int CompareTo(IncludedIndexValue other)
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

            var other = (IncludedIndexValue)obj;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}
