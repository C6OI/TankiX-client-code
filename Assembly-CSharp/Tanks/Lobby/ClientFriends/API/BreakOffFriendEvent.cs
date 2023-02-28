using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientFriends.Impl;

namespace Tanks.Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1450264928332L)]
    public class BreakOffFriendEvent : FriendBaseEvent {
        public BreakOffFriendEvent() { }

        public BreakOffFriendEvent(Entity user)
            : base(user) { }
    }
}