using System;

namespace Platform.Library.ClientDataStructures.Impl {
    public class TypeIsNotComparableException : InvalidOperationException {
        public TypeIsNotComparableException(Type type)
            : base(string.Format("Type {0} is not derived from IComparable.", type)) { }
    }
}