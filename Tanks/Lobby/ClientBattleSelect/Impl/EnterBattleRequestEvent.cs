using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(-4518638013450931090L)]
    [Shared]
    public class EnterBattleRequestEvent : Event {
        public EnterBattleRequestEvent() { }

        public EnterBattleRequestEvent(TeamColor teamColor) => TeamColor = teamColor;

        public TeamColor TeamColor { get; set; }
    }
}