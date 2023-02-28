using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ReturnToBattleTextComponent : Component {
        public string DialogHeader { get; set; }

        public string DialogMainText { get; set; }

        public string DialogOk { get; set; }

        public string DialogNo { get; set; }
    }
}