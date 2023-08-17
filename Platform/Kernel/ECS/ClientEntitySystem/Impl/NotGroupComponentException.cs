using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NotGroupComponentException : Exception {
        public NotGroupComponentException(Type type)
            : base("Type: " + type) { }
    }
}