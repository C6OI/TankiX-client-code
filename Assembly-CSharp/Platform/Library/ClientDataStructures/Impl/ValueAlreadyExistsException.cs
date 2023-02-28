using System;

namespace Platform.Library.ClientDataStructures.Impl {
    public class ValueAlreadyExistsException : ArgumentException {
        public ValueAlreadyExistsException(object value)
            : base(string.Format("Value {0} already exists in map.", value)) { }
    }
}