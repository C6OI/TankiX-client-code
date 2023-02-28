using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1507725575587L)]
    public class FriendInviteToSquadEvent : Event {
        public FriendInviteToSquadEvent(long userId, InteractionSource interactionSource, long sourceId) {
            UserId = userId;
            InteractionSource = interactionSource;
            SourceId = sourceId;
        }

        public long UserId { get; set; }

        public InteractionSource InteractionSource { get; set; }

        public long SourceId { get; set; }
    }
}