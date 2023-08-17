using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class DuplicateComponentOnTemplateException : Exception {
        public DuplicateComponentOnTemplateException(TemplateDescription templateDescription, Type componentType)
            : base(string.Concat("templateDescription=", templateDescription, " componentType=", componentType)) { }
    }
}