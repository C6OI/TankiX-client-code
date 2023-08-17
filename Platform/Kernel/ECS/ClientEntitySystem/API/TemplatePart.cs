using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class TemplatePart : Attribute { }
}