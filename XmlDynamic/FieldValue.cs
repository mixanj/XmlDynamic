namespace Dynamic.Xml
{
    using System;

    public class FieldValue
    {
        public Guid Id { get; set; }

        public virtual dynamic GetValue() { return null; }
    }

    public class FieldValue<T> : FieldValue
    {
        public FieldValue(Guid id, object value)
        {
            Id = id;
            Value = (T)value;
        }

        public T Value { get; set; }

        public override dynamic GetValue()
        {
            return Value;
        }
    }
}
