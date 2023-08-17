using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class CannotRegisterTemplatePartAsTemplateException : Exception {
        public CannotRegisterTemplatePartAsTemplateException(Type templateClass)
            : base("templateClass=" + templateClass) { }
    }
}