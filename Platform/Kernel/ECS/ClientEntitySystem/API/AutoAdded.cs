using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AutoAdded : Attribute { }
}