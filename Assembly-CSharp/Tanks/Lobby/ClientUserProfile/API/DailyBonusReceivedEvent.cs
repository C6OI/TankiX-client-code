using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(636458162767978928L)]
    public class DailyBonusReceivedEvent : Event {
        public long Code { get; set; }
    }
}