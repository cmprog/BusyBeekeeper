namespace BusyBeekeeper
{
    public sealed class SharedProperty<TValue>
    {
        public SharedProperty(TValue value) { this.Value = value; }
        public TValue Value { get; set; }
    }

    public static class SharedProperty
    {
        public static SharedProperty<TValue> Create<TValue>(TValue value)
        {
            return new SharedProperty<TValue>(value);
        }
    }
}
