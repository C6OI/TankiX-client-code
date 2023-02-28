using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1503654626834L)]
    public class CurrentSeasonRewardForClientComponent : Component {
        public List<EndSeasonRewardItem> Rewards { get; set; }
    }
}