using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class WeaponCooldownSystem : AbstractWeaponCooldownSystem {
        [OnEventFire]
        public void ResetOnActivate(NodeAddedEvent e, ActiveTankNode activeTank, [JoinByTank] CooldownNode cooldown) {
            cooldown.cooldownTimer.CooldownTimerSec = 0f;
        }

        [OnEventFire]
        public void DecreaseCooldownTimer(TimeUpdateEvent evt, CooldownNode cooldown) {
            cooldown.cooldownTimer.CooldownTimerSec = Mathf.Max(cooldown.cooldownTimer.CooldownTimerSec - evt.DeltaTime, 0f);
        }

        [OnEventFire]
        public void DefineCooldownTimerForNextPossibleShot(ShotPrepareEvent evt, CooldownNode cooldown) {
            UpdateCooldownOnShot(cooldown.cooldownTimer, cooldown.weaponCooldown);
        }

        public class CooldownNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public WeaponCooldownComponent weaponCooldown;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;
        }
    }
}