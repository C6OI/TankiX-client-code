using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ComponentNotFoundInTemplateException : Exception {
        public ComponentNotFoundInTemplateException(Type componentClass, string templateName)
            : base("template= " + templateName + " component= " + componentClass.FullName) { }
    }
}