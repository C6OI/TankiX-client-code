using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(636459034861529826L)]
    public class UserDailyBonusCycleComponent : Component {
        public long CycleNumber { get; set; }
    }
}