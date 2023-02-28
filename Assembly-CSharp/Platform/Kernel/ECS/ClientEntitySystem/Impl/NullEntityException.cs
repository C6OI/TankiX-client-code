using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NullEntityException : Exception {
        public NullEntityException()
            : base("Events can not be scheduled on null entities") { }
    }
}