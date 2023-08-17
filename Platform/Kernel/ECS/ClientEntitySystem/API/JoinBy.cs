using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    public class JoinBy : Attribute {
        internal readonly Type value;

        public JoinBy(Type value) => this.value = value;
    }
}