using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(635906273125139964L)]
    public class CheckEmailEvent : Event {
        public CheckEmailEvent() { }

        public CheckEmailEvent(string email) => Email = email;

        public CheckEmailEvent(string email, bool includeUnconfirmed) {
            Email = email;
            IncludeUnconfirmed = includeUnconfirmed;
        }

        public string Email { get; set; }

        public bool IncludeUnconfirmed { get; set; }
    }
}