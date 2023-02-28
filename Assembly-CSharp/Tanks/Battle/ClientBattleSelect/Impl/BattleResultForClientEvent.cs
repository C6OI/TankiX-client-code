using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1510029455297L)]
    public class BattleResultForClientEvent : Event {
        public BattleResultForClient UserResultForClient { get; set; }
    }
}