using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1502790165197L)]
    public class AuthenticateSteamUserEvent : Event {
        public string SteamId { get; set; }

        public string Ticket { get; set; }

        public string HardwareFingerpring { get; set; }

        public string SteamNickname { get; set; }
    }
}