using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ChangeHUDModeEvent : Event {
        public SpectatorHUDMode Mode { get; set; }
    }
}