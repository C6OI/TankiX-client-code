using System;

namespace Platform.Library.ClientDataStructures.API {
    public class ClassNotFoundException : Exception {
        public ClassNotFoundException(long id)
            : base("Id = " + id) { }
    }
}