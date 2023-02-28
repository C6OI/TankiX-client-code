using System;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AnnotationComponentInfoBuilder<T> : ComponentInfoBuilder where T : ComponentInfo, new() {
        readonly Type annotationType;

        public AnnotationComponentInfoBuilder(Type annotationType, Type componentInfoClass) => this.annotationType = annotationType;

        public Type TemplateComponentInfoClass => typeof(T);

        public bool IsAcceptable(MethodInfo componentMethod) => componentMethod.GetCustomAttributes(annotationType, true).Length == 1;

        public ComponentInfo Build(MethodInfo componentMethod, ComponentDescriptionImpl componentDescription) => new T();
    }
}