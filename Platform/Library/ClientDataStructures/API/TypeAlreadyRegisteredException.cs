using System;

namespace Platform.Library.ClientDataStructures.API {
    public class TypeAlreadyRegisteredException : Exception {
        public TypeAlreadyRegisteredException(Type type)
            : base("Class=" + type) { }
    }
}