using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class UnsupportedParameterException : Exception {
        public UnsupportedParameterException(Type type)
            : base("type = " + type) { }
    }
}