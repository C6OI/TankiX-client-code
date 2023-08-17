using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.Impl {
    public class ResourceWarmupIndexComponent : Component {
        public ResourceWarmupIndexComponent() => Index = 0;

        public int Index { get; set; }
    }
}