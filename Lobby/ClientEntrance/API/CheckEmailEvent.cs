using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(635906273125139964L)]
    [Shared]
    public class CheckEmailEvent : Event {
        public CheckEmailEvent() { }

        public CheckEmailEvent(string email) => Email = email;

        public string Email { get; set; }
    }
}