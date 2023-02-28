using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(1437990639822L)]
    public class CheckUserUidEvent : Event {
        public CheckUserUidEvent() { }

        public CheckUserUidEvent(string uid) => Uid = uid;

        public string Uid { get; set; }
    }
}