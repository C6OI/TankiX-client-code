using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1412360987645L)]
    public class UserInteractionDataResponseEvent : Event {
        public long UserId { get; set; }

        public string UserUid { get; set; }

        public bool CanRequestFrendship { get; set; }

        public bool FriendshipRequestWasSend { get; set; }

        public bool Muted { get; set; }

        public bool Reported { get; set; }
    }
}