using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ShowXCrystalsDialogEvent : Event {
        public bool ShowTitle { get; set; }
    }
}