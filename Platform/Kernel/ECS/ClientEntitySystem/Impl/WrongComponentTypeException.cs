using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class WrongComponentTypeException : Exception {
        public WrongComponentTypeException(Type componentType)
            : base("componentType=" + componentType) { }
    }
}