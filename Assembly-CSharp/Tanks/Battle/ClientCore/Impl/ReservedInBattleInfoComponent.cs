using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1490682041080L)]
    public class ReservedInBattleInfoComponent : Component {
        public Date ExitTime { get; set; }

        public long Map { get; set; }

        public BattleMode BattleMode { get; set; }
    }
}