using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AutoAddedComponentInfoBuilder : AnnotationComponentInfoBuilder<AutoAddedComponentInfo> {
        public AutoAddedComponentInfoBuilder()
            : base(typeof(AutoAdded), typeof(AutoAddedComponentInfo)) { }
    }
}