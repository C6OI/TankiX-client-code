using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(636464215401773226L)]
    public class ReceiveTargetItemFromDetailsByDailyBonusEvent : Event {
        public long DetailMarketItemId { get; set; }
    }
}