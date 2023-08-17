using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    public class JoinAll : Attribute { }
}