using System;

namespace IndexesPostgreSQL
{
    public class PartialIndexValue : IComparable<PartialIndexValue>
    {
        public int? Value { get; set; }

        public PartialIndexValue(int? value)
        {
            Value = value;
        }

        public int CompareTo(PartialIndexValue other)
        {
            return Nullable.Compare(this.Value, other.Value);
        }

        public override string ToString()
        {
            if (Value == null) return "null";
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            var value = obj as PartialIndexValue;
            if (this.Value == null && value.Value == null) return true;
            else if (this.Value.Value == value.Value.Value) return true;
            else return false;
        }
    }
}
