using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(636389758870600269L)]
    public class GameplayChestScoreComponent : Component {
        public long Current { get; set; }

        public long Limit { get; set; }
    }
}