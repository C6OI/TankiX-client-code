using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerException : Exception {
        public HandlerException(string message, Exception cause)
            : base(message, cause) { }
    }
}