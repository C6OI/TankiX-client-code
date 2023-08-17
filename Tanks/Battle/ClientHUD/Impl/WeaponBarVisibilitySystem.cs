using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class WeaponBarVisibilitySystem : ECSSystem {
        static readonly string PARTIALLY_FILLED_BAR_PREREQUISITE = "PARTIALLY_FILLED_BAR_PREREQUISITE";

        [OnEventFire]
        public void HideProgressBar(NodeRemoveEvent e, SelfTankActiveNode tank, [JoinAll] WeaponBarNode weaponBar) =>
            weaponBar.visibilityPrerequisites.RemoveShowPrerequisite(PARTIALLY_FILLED_BAR_PREREQUISITE);

        [OnEventFire]
        public void UpdateVisibility(UpdateEvent e, WeaponBarNode weaponBar, [JoinAll] SelfTankActiveNode tank) {
            VisibilityPrerequisitesComponent visibilityPrerequisites = weaponBar.visibilityPrerequisites;

            if (weaponBar.weaponBar.ProgressValue < 1f) {
                visibilityPrerequisites.AddShowPrerequisite(PARTIALLY_FILLED_BAR_PREREQUISITE);
            } else {
                visibilityPrerequisites.RemoveShowPrerequisite(PARTIALLY_FILLED_BAR_PREREQUISITE);
            }
        }

        public class WeaponBarNode : Node {
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
            public WeaponBarComponent weaponBar;
        }

        public class SelfTankActiveNode : Node {
            public SelfTankComponent selfTank;

            public TankActiveStateComponent tankActiveState;
        }
    }
}