using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class EntityStateAlreadyRegisteredException : Exception {
        public EntityStateAlreadyRegisteredException(Type stateType)
            : base(string.Concat("State ", stateType, " is not registered")) { }
    }
}