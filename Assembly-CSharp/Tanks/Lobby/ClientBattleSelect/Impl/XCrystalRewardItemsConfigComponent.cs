using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(636493662828220115L)]
    public class XCrystalRewardItemsConfigComponent : Component {
        public string SpriteUid { get; set; }

        public string Title { get; set; }
    }
}