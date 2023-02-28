using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1477557707454L)]
    public class EnterRelevantBattleEvent : Event {
        public TeamColor PreferredTeam { get; set; }

        public long PreferredBattle { get; set; }

        public string Source { get; set; }
    }
}