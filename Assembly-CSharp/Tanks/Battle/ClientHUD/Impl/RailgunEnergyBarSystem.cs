using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class RailgunEnergyBarSystem : ECSSystem {
        [OnEventFire]
        public void AddAnimationData(NodeAddedEvent e, RailgunChargingEnergyNode weapon, [JoinByTank] [Context] HUDNodes.SelfTankNode tank) {
            if (!weapon.Entity.HasComponent<ChargeAnimationDataComponent>()) {
                weapon.Entity.AddComponent<ChargeAnimationDataComponent>();
            }

            weapon.Entity.GetComponent<ChargeAnimationDataComponent>().CurrentDuration = 0f;
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, RailgunEnergyNode weapon, [JoinByTank] [Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud) {
            hud.component.EnergyBarEnabled = true;
            hud.component.MaxEnergyValue = 1f;
            hud.component.CurrentEnergyValue = 0f;
            hud.component.EnergyAmountPerSegment = 1f;

            if (weapon.Entity.HasComponent<ChargeAnimationDataComponent>()) {
                weapon.Entity.RemoveComponent<ChargeAnimationDataComponent>();
            }
        }

        [OnEventFire]
        public void Update(TimeUpdateEvent e, RailgunReadyEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            float currentEnergyValue = hud.component.CurrentEnergyValue;
            hud.component.CurrentEnergyValue = weapon.weaponEnergy.Energy;

            if (hud.component.CurrentEnergyValue == hud.component.MaxEnergyValue && currentEnergyValue < hud.component.MaxEnergyValue) {
                hud.component.EnergyBlink(true);
            }
        }

        [OnEventFire]
        public void UpdateCharging(TimeUpdateEvent e, RailgunChargingEnergyAnimatedNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank,
            [JoinAll] SingleNode<MainHUDComponent> hud) {
            float chargingTime = weapon.railgunChargingWeapon.ChargingTime;
            hud.component.CurrentEnergyValue = EaseInQuad(weapon.chargeAnimationData.CurrentDuration / chargingTime, 1f, -1f);
            weapon.chargeAnimationData.CurrentDuration += e.DeltaTime;
        }

        float EaseInQuad(float time, float from, float delta) => delta * time * time + from;

        public class RailgunEnergyNode : Node {
            public RailgunChargingWeaponComponent railgunChargingWeapon;
            public RailgunEnergyBarComponent railgunEnergyBar;

            public TankGroupComponent tankGroup;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class RailgunReadyEnergyNode : RailgunEnergyNode {
            public ReadyRailgunChargingWeaponComponent readyRailgunChargingWeapon;
        }

        public class RailgunChargingEnergyNode : RailgunEnergyNode {
            public RailgunChargingStateComponent railgunChargingState;
        }

        public class RailgunChargingEnergyAnimatedNode : RailgunChargingEnergyNode {
            public ChargeAnimationDataComponent chargeAnimationData;
        }

        public class ChargeAnimationDataComponent : Component {
            public float CurrentDuration { get; set; }
        }
    }
}