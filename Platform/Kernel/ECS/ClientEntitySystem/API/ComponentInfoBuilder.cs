using System;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface ComponentInfoBuilder {
        Type TemplateComponentInfoClass { get; }

        bool IsAcceptable(MethodInfo componentMethod);

        ComponentInfo Build(MethodInfo componentMethod, ComponentDescriptionImpl componentDescription);
    }
}