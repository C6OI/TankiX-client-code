using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class StreamWeaponCooldownSystem : ECSSystem {
        [OnEventFire]
        public void DefineCooldownTimerForTheFirstTickInWorkingState(NodeAddedEvent evt, StreamWeaponCooldownNode weapon) =>
            weapon.cooldownTimer.CooldownTimerSec = weapon.weaponCooldown.CooldownIntervalSec;

        public class StreamWeaponCooldownNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public StreamWeaponComponent streamWeapon;

            public StreamWeaponWorkingComponent streamWeaponWorking;

            public WeaponCooldownComponent weaponCooldown;
        }
    }
}