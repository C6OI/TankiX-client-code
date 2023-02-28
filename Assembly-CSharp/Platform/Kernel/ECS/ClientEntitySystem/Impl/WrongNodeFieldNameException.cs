using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class WrongNodeFieldNameException : Exception {
        public WrongNodeFieldNameException(Type nodeClass, Type fieldType, string fieldName)
            : base(string.Format("node = {0}, fieldType = {1}, fieldName = {2}", nodeClass, fieldType.Name, fieldName)) { }
    }
}