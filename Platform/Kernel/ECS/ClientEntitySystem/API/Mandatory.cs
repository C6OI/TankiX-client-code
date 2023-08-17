using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, Inherited = false)]
    public class Mandatory : Attribute { }
}