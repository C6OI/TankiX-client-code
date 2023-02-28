using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class StreamEnergyBarSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void Init(NodeAddedEvent e, StreamEnergyNode weapon, [JoinByTank] [Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud) {
            hud.component.EnergyBarEnabled = true;
            hud.component.MaxEnergyValue = 1f;
            hud.component.CurrentEnergyValue = 0f;
            hud.component.EnergyAmountPerSegment = 1f;
        }

        [OnEventFire]
        public void Energy(TimeUpdateEvent e, StreamEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            hud.component.CurrentEnergyValue = weapon.weaponEnergy.Energy;

            if (InputManager.GetActionKeyDown(ShotActions.SHOT) && !weapon.Entity.HasComponent<ShootableComponent>()) {
                hud.component.EnergyBlink(false);
            }
        }

        [Not(typeof(TwinsComponent))]
        public class StreamEnergyNode : HUDNodes.BaseWeaponNode {
            public StreamWeaponComponent streamWeapon;
        }
    }
}