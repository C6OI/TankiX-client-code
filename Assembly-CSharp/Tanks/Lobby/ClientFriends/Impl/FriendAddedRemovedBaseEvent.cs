using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendAddedRemovedBaseEvent : Event {
        public long FriendId { get; set; }
    }
}