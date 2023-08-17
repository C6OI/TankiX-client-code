using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1431927384785L)]
    public class MineConfigComponent : Component {
        public float DamageFrom { get; set; }

        public float DamageTo { get; set; }

        public long ActivationTime { get; set; }

        public float Impact { get; set; }

        public float BeginHideDistance { get; set; }

        public float HideRange { get; set; }
    }
}