using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class EntityStateNotRegisteredException : Exception {
        public EntityStateNotRegisteredException(Type type)
            : base(string.Concat("State ", type, " is not registered")) { }
    }
}