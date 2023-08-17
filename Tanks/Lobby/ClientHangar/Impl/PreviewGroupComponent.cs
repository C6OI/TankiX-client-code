using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class PreviewGroupComponent : GroupComponent {
        public PreviewGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public PreviewGroupComponent(long key)
            : base(key) { }
    }
}