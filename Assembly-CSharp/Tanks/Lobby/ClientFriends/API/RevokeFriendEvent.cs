using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientFriends.Impl;

namespace Tanks.Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1450263956353L)]
    public class RevokeFriendEvent : FriendBaseEvent {
        public RevokeFriendEvent() { }

        public RevokeFriendEvent(Entity user)
            : base(user) { }
    }
}