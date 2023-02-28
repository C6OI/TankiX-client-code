using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class MissingTemplatePartAttributeException : Exception {
        public MissingTemplatePartAttributeException(Type templatePartClass)
            : base("class=" + templatePartClass) { }
    }
}