using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    [Shared]
    [SerialVersionUID(1450343409998L)]
    public class FriendBaseEvent : Event {
        public FriendBaseEvent() { }

        public FriendBaseEvent(Entity user) => User = user;

        public Entity User { get; set; }
    }
}