using System;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class MissingHandlerAnnotationException : Exception {
        public MissingHandlerAnnotationException(MethodInfo method)
            : base(string.Concat("method = ", method, ", class = ", method.DeclaringType)) { }
    }
}