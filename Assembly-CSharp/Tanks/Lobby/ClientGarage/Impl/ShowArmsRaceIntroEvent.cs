using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ShowArmsRaceIntroEvent : Event {
        public ConfirmDialogComponent Dialog { get; set; }
    }
}