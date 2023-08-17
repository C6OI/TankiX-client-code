using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class CannotAccessPathForTemplate : Exception {
        public CannotAccessPathForTemplate(Type templateClass)
            : base(templateClass.FullName) { }
    }
}