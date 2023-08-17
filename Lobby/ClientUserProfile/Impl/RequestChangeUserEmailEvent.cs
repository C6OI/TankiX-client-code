using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1457935367814L)]
    public class RequestChangeUserEmailEvent : Event {
        public RequestChangeUserEmailEvent() { }

        public RequestChangeUserEmailEvent(string email) => Email = email;

        public string Email { get; set; }
    }
}