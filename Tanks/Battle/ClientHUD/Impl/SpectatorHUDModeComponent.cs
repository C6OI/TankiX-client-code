using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class SpectatorHUDModeComponent : Component {
        public SpectatorHUDModeComponent() { }

        public SpectatorHUDModeComponent(SpectatorHUDMode hudMode) => HUDMode = hudMode;

        public SpectatorHUDMode HUDMode { get; set; }
    }
}