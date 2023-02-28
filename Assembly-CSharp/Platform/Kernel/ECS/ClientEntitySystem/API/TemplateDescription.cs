using System;
using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface TemplateDescription {
        long TemplateId { get; }

        string TemplateName { get; }

        ICollection<ComponentDescription> ComponentDescriptions { get; }

        Type TemplateClass { get; }

        bool IsOverridedConfigPath();

        string OverrideConfigPath(string path);

        bool IsComponentDescriptionPresent(Type componentType);

        ComponentDescription GetComponentDescription(Type componentType);

        ICollection<Type> GetAutoAddedComponentTypes();
    }
}