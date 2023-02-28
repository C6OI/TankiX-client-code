using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1507198221820L)]
    public class ChangeBlockStateByUserIdRequest : Event {
        public ChangeBlockStateByUserIdRequest(long userId, InteractionSource interactionSource, long sourceId) {
            UserId = userId;
            InteractionSource = interactionSource;
            SourceId = sourceId;
        }

        public long UserId { get; set; }

        public InteractionSource InteractionSource { get; set; }

        public long SourceId { get; set; }
    }
}