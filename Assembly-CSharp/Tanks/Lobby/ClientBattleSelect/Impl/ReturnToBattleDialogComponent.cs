using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ReturnToBattleDialogComponent : BehaviourComponent {
        public int SecondsLeft { get; set; }

        public string PreformatedMainText { get; set; }
    }
}