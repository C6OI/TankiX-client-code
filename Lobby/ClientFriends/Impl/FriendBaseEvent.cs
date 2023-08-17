using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.Impl {
    [SerialVersionUID(1450343409998L)]
    [Shared]
    public class FriendBaseEvent : Event {
        public FriendBaseEvent() { }

        public FriendBaseEvent(Entity user) => User = user;

        public Entity User { get; set; }
    }
}