using Lobby.ClientFriends.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1450168274692L)]
    public class RejectFriendEvent : FriendBaseEvent {
        public RejectFriendEvent() { }

        public RejectFriendEvent(Entity user)
            : base(user) { }
    }
}