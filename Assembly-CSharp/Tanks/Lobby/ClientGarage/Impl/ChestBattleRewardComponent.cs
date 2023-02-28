using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(636390744977660302L)]
    public class ChestBattleRewardComponent : Component {
        public long ChestId { get; set; }
    }
}