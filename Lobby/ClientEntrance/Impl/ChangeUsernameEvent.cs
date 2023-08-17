using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(636071969143771105L)]
    [Shared]
    public class ChangeUsernameEvent : Event {
        public ChangeUsernameEvent() { }

        public ChangeUsernameEvent(string uid) => Uid = uid;

        public string Uid { get; set; }
    }
}