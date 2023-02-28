using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(4788927540455272293L)]
    public class UserInBattleAsSpectatorComponent : Component {
        public long BattleId { get; set; }
    }
}