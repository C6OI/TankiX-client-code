using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(636498167526154024L)]
    public class LevelUpUnlockRewardConfigComponent : Component {
        public string ActiveSlotSpriteUid { get; set; }

        public string PassiveSlotSpriteUid { get; set; }

        public string ActiveSlotWeaponText { get; set; }

        public string ActiveSlotHullText { get; set; }

        public string PassiveSlotWeaponText { get; set; }

        public string PassiveSlotHullText { get; set; }
    }
}