using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class GraphicsSettingsBuilderGroupComponent : GroupComponent {
        public GraphicsSettingsBuilderGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public GraphicsSettingsBuilderGroupComponent(long key)
            : base(key) { }
    }
}