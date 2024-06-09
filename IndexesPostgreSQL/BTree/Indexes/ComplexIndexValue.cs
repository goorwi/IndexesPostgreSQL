using System;

namespace IndexesPostgreSQL
{
    public class ComplexIndexValue : IComparable<ComplexIndexValue>
    {
        public int? FirstValue { get; set; }
        public int? SecondValue { get; set; }

        public ComplexIndexValue(int? firstValue, int? secondValue)
        {
            FirstValue = firstValue;
            SecondValue = secondValue;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (ComplexIndexValue)obj;

            return FirstValue == other.FirstValue && SecondValue == other.SecondValue;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (FirstValue?.GetHashCode() ?? 0);
                hash = hash * 23 + (SecondValue?.GetHashCode() ?? 0);
                return hash;
            }
        }

        public int CompareTo(ComplexIndexValue other)
        {
            if (other == null) return 1;

            int firstComparison = Nullable.Compare(FirstValue, other.FirstValue);
            if (firstComparison != 0)
                return firstComparison;

            return Nullable.Compare(SecondValue, other.SecondValue);
        }

        public override string ToString()
        {
            var first = FirstValue?.ToString() ?? "null";
            var second = SecondValue?.ToString() ?? "null";
            return $"({first} / {second})";
        }
    }
}
