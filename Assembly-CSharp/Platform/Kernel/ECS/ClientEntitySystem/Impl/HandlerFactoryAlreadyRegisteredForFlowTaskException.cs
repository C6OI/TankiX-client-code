using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerFactoryAlreadyRegisteredForFlowTaskException : Exception {
        public HandlerFactoryAlreadyRegisteredForFlowTaskException(Type taskType)
            : base(taskType.FullName) { }
    }
}