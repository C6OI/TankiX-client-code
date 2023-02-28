using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(636492940634270399L)]
    public class XCrystalRewardTextConfigComponent : Component {
        public IDictionary<XCrystalBonusActivationReason, string> Title { get; set; }

        public IDictionary<XCrystalBonusActivationReason, string> Description { get; set; }

        public string PurchaseText { get; set; }
    }
}