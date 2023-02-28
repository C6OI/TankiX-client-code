using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(636464291410970703L)]
    public class TargetItemFromDailyBonusReceivedEvent : Event {
        public long DetailMarketItemId { get; set; }
    }
}