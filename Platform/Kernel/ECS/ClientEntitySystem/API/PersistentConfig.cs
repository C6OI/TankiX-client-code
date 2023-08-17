using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class PersistentConfig : Attribute {
        internal readonly bool configOptional;
        internal readonly string value;

        public PersistentConfig(string value = "", bool configOptional = false) {
            this.value = value;
            this.configOptional = configOptional;
        }
    }
}