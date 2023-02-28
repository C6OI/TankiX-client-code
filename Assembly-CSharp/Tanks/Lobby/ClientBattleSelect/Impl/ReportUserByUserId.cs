using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1506939739582L)]
    public class ReportUserByUserId : Event {
        public ReportUserByUserId(long userId, InteractionSource interactionSource, long sourceId) {
            UserId = userId;
            InteractionSource = interactionSource;
            SourceId = sourceId;
        }

        public long UserId { get; set; }

        public InteractionSource InteractionSource { get; set; }

        public long SourceId { get; set; }
    }
}