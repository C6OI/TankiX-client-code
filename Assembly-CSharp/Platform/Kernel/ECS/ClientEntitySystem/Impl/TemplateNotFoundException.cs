using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TemplateNotFoundException : Exception {
        public TemplateNotFoundException(long id)
            : base("templateId=" + id) { }
    }
}