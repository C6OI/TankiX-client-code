namespace Platform.Library.ClientDataStructures.Impl {
    public class ValueNotFoundException : global::System.Collections.Generic.KeyNotFoundException {
        public ValueNotFoundException(object value)
            : base(string.Format("Value {0} not found in map.", value)) { }
    }
}