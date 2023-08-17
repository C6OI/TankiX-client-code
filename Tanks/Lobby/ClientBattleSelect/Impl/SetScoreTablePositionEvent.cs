using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(1463648611538L)]
    [Shared]
    public class SetScoreTablePositionEvent : Event {
        public SetScoreTablePositionEvent() { }

        public SetScoreTablePositionEvent(int position) => Position = position;

        public int Position { get; set; }
    }
}