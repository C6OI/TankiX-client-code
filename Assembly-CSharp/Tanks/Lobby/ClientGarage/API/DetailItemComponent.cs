using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636457322095664962L)]
    public class DetailItemComponent : Component {
        public long TargetMarketItemId { get; set; }

        public long RequiredCount { get; set; }
    }
}