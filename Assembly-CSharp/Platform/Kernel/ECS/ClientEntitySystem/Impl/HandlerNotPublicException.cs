using System;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerNotPublicException : Exception {
        public HandlerNotPublicException(MethodInfo method)
            : base("method=" + method) { }
    }
}