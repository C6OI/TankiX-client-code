using System;

namespace Platform.Library.ClientDataStructures.Impl {
    public class KeyAlreadyExistsException : ArgumentException {
        public KeyAlreadyExistsException(object key)
            : base(string.Format("Key {0} already exists in map.", key)) { }
    }
}