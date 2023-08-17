using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientFriends.Impl {
    public class FriendAddedRemovedBaseEvent : Event {
        public long FriendId { get; set; }
    }
}