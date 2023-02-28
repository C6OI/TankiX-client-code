using System;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerFactoryException : Exception {
        public HandlerFactoryException(MethodInfo method)
            : base("Method: " + method) { }

        public HandlerFactoryException(MethodInfo method, Type type)
            : base(string.Concat("Method: ", method, ", type: ", type)) { }
    }
}