using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(636458159324341964L)]
    public class ReceiveDailyBonusEvent : Event {
        public long Code { get; set; }
    }
}