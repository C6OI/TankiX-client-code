using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [SerialVersionUID(1502954725589L)]
    public class SteamAuthSessionRecievedEvent : Event {
        public bool GoToPayment { get; set; }
    }
}