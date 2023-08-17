using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class SystemAlreadyRegisteredException : Exception {
        public SystemAlreadyRegisteredException(Type systemType)
            : base("system = " + systemType) { }
    }
}