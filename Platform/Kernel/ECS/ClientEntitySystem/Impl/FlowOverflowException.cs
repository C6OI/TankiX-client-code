using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FlowOverflowException : Exception {
        public FlowOverflowException(string stackTrace)
            : base(stackTrace) { }
    }
}