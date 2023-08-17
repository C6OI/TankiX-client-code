using Lobby.ClientFriends.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.API {
    [SerialVersionUID(1450263956353L)]
    [Shared]
    public class RevokeFriendEvent : FriendBaseEvent {
        public RevokeFriendEvent() { }

        public RevokeFriendEvent(Entity user)
            : base(user) { }
    }
}