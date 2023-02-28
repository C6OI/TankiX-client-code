using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1502092676956L)]
    public class BattleLeaveCounterComponent : Component {
        public long Value { get; set; }

        public int NeedGoodBattles { get; set; }
    }
}