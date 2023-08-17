using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TemplatePartShouldExtendSingleTemplateException : Exception {
        public TemplatePartShouldExtendSingleTemplateException(Type templatePartClass, Type baseClass)
            : base(string.Concat("templatePartClass=", templatePartClass, " baseClass=", baseClass)) { }

        public TemplatePartShouldExtendSingleTemplateException(Type templatePartClass)
            : base("templatePartClass=" + templatePartClass) { }
    }
}