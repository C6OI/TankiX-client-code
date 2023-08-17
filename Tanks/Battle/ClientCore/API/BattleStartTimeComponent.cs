using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1436521738148L)]
    [Shared]
    public class BattleStartTimeComponent : Component {
        [ProtocolOptional] public Date RoundStartTime { get; set; }
    }
}