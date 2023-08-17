using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class UnknownGroupContainerException : Exception {
        public UnknownGroupContainerException(Type containerClass)
            : base("containerClass=" + containerClass) { }
    }
}