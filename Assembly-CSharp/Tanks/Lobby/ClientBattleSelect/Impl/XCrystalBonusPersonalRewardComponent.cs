using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1513666203904L)]
    public class XCrystalBonusPersonalRewardComponent : Component {
        public XCrystalBonusActivationReason ActivationReason { get; set; }

        public double Multiplier { get; set; }
    }
}