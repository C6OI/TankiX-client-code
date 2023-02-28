using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1502879856557L)]
    public class SteamUserComponent : Component {
        public string SteamId { get; set; }

        public bool FreeUidChange { get; set; }
    }
}