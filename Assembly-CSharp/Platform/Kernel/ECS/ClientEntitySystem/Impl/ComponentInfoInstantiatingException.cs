using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ComponentInfoInstantiatingException : Exception {
        public ComponentInfoInstantiatingException(Type componentInfoClass, Exception e)
            : base(string.Concat("component info class = ", componentInfoClass, ", message = ", e.Message), e) { }
    }
}