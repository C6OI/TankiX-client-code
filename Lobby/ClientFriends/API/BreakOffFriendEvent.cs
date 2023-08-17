using Lobby.ClientFriends.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.API {
    [SerialVersionUID(1450264928332L)]
    [Shared]
    public class BreakOffFriendEvent : FriendBaseEvent {
        public BreakOffFriendEvent() { }

        public BreakOffFriendEvent(Entity user)
            : base(user) { }
    }
}