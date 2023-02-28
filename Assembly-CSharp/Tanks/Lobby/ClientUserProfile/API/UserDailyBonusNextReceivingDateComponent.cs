using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(636462622709176439L)]
    public class UserDailyBonusNextReceivingDateComponent : Component {
        [ProtocolOptional] public Date Date { get; set; }

        public long TotalMillisLength { get; set; }
    }
}