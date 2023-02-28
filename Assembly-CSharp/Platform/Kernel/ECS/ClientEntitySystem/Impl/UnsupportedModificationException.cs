using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class UnsupportedModificationException : Exception {
        public UnsupportedModificationException(Entity currentKey, Entity newKey)
            : base(string.Concat("currentKey = ", currentKey, ", newKey = ", newKey)) { }
    }
}