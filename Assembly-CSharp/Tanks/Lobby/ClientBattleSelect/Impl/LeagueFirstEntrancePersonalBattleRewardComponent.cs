using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1513680986907L)]
    public class LeagueFirstEntrancePersonalBattleRewardComponent : Component {
        public Entity PersonalOffer { get; set; }
    }
}