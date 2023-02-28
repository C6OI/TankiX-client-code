using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftAimingCooldownSystem : AbstractWeaponCooldownSystem {
        [OnEventFire]
        public void DefineCooldownTimerForNextPossibleShot(ShaftAimingShotPrepareEvent evt, CooldownNode cooldown) {
            UpdateCooldownOnShot(cooldown.cooldownTimer, cooldown.weaponCooldown);
        }

        public class CooldownNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public WeaponCooldownComponent weaponCooldown;
        }
    }
}