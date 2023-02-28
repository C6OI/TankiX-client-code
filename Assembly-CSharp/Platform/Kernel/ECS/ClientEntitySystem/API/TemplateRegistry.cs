using System;
using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface TemplateRegistry {
        ICollection<ComponentInfoBuilder> ComponentInfoBuilders { get; }

        TemplateDescription GetTemplateInfo(long id);

        TemplateDescription GetTemplateInfo(Type templateType);

        void Register(Type templateClass);

        void Register<T>() where T : Template;

        void RegisterPart(Type templatePartClass);

        void RegisterPart<T>() where T : Template;

        void RegisterComponentInfoBuilder(ComponentInfoBuilder componentInfoBuilder);

        ICollection<Type> GetParentTemplateClasses(Type templateType);
    }
}