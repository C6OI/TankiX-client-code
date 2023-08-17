namespace Platform.Library.ClientDataStructures.Impl {
    public class KeyNotFoundException : global::System.Collections.Generic.KeyNotFoundException {
        public KeyNotFoundException(object key)
            : base(string.Format("Key {0} not found in map.", key)) { }
    }
}