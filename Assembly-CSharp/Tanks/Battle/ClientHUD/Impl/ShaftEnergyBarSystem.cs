using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ShaftEnergyBarSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void Init(NodeAddedEvent e, ShaftEnergyNode weapon, [JoinByTank] [Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud) {
            hud.component.EnergyBarEnabled = true;
            hud.component.MaxEnergyValue = 1f;
            hud.component.CurrentEnergyValue = 0f;
            hud.component.EnergyAmountPerSegment = 1f;
        }

        [OnEventFire]
        public void UpdateEnergy(TimeUpdateEvent e, ShaftEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            float unloadEnergyPerQuickShot = weapon.shaftEnergy.UnloadEnergyPerQuickShot;
            float energy = weapon.weaponEnergy.Energy;
            CooldownTimerComponent cooldownTimer = weapon.cooldownTimer;

            if (energy < unloadEnergyPerQuickShot || cooldownTimer.CooldownTimerSec > 0f || !weapon.Entity.HasComponent<ShootableComponent>()) {
                if (weapon.Entity.HasComponent<BlinkMarkerComponent>()) {
                    weapon.Entity.RemoveComponent<BlinkMarkerComponent>();
                }

                if (InputManager.GetActionKeyDown(ShotActions.SHOT)) {
                    hud.component.EnergyBlink(false);
                }
            } else if (!weapon.Entity.HasComponent<BlinkMarkerComponent>()) {
                weapon.Entity.AddComponent<BlinkMarkerComponent>();
                hud.component.EnergyBlink(true);
            }

            hud.component.CurrentEnergyValue = weapon.weaponEnergy.Energy;
        }

        public class ShaftEnergyNode : Node {
            public CooldownTimerComponent cooldownTimer;
            public ShaftEnergyComponent shaftEnergy;

            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class ShaftReloadingEnergyNode : WeaponEnergyStates.WeaponEnergyReloadingState {
            public ShaftEnergyComponent shaftEnergy;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class ShaftReadyEnergyNode : ShaftEnergyNode {
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class BlinkMarkerComponent : Component { }
    }
}