using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class VulcanEnergyBarSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [Inject] public static UnityTime Time { get; set; }

        [OnEventFire]
        public void Init(NodeAddedEvent e, VulcanWeaponNode vulcan, [JoinByTank] [Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud) {
            hud.component.EnergyBarEnabled = true;
            hud.component.EnergyAmountPerSegment = 1f;
            hud.component.MaxEnergyValue = 2f;
            hud.component.CurrentEnergyValue = 0f;
        }

        [OnEventFire]
        public void Working(TimeUpdateEvent e, VulcanWeaponWorkingNode vulcan, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            float num = Date.Now - vulcan.weaponStreamShooting.StartShootingTime;
            float temperatureHittingTime = vulcan.vulcanWeapon.TemperatureHittingTime;
            float num2 = temperatureHittingTime - num;

            if (num2 < 0f) {
                num2 = 0f;
                hud.component.EnergyBlink(false);
            }

            hud.component.CurrentEnergyValue = vulcan.vulcanWeaponState.State + 1f - num2 / temperatureHittingTime;
        }

        [OnEventFire]
        public void SlowDown(TimeUpdateEvent e, VulcanWeaponSlowDownNode vulcan, [JoinByTank] SingleNode<AnimationDataComponent> animData, [JoinByTank] HUDNodes.ActiveSelfTankNode tank,
            [JoinAll] SingleNode<MainHUDComponent> hud) {
            hud.component.CurrentEnergyValue = vulcan.vulcanWeaponState.State * animData.component.CooldownScale;

            if (InputManager.GetActionKeyDown(ShotActions.SHOT)) {
                hud.component.EnergyBlink(false);
            }
        }

        [OnEventFire]
        public void SlowDown(NodeAddedEvent e, VulcanWeaponSlowDownNode vulcan, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            hud.component.StopEnergyBlink();

            if (vulcan.Entity.HasComponent<AnimationDataComponent>()) {
                vulcan.Entity.RemoveComponent<AnimationDataComponent>();
            }

            vulcan.Entity.AddComponent(new AnimationDataComponent {
                CooldownScale = hud.component.CurrentEnergyValue
            });
        }

        [OnEventFire]
        public void Idle(TimeUpdateEvent e, VulcanWeaponIdleNode vulcan, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            hud.component.CurrentEnergyValue = 0f;
        }

        [OnEventFire]
        public void SpeedUp(TimeUpdateEvent e, VulcanWeaponSpeedUpNode vulcan, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            hud.component.CurrentEnergyValue = vulcan.vulcanWeaponState.State;
        }

        public class VulcanWeaponNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public TankGroupComponent tankGroup;

            public VulcanEnergyBarComponent vulcanEnergyBar;
            public VulcanWeaponComponent vulcanWeapon;

            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanWeaponWorkingNode : VulcanWeaponNode {
            public WeaponStreamShootingComponent weaponStreamShooting;
        }

        public class VulcanWeaponSlowDownNode : VulcanWeaponNode {
            public VulcanSlowDownComponent vulcanSlowDown;
        }

        public class VulcanWeaponIdleNode : VulcanWeaponNode {
            public VulcanIdleComponent vulcanIdle;
        }

        public class VulcanWeaponSpeedUpNode : VulcanWeaponNode {
            public VulcanSpeedUpComponent vulcanSpeedUp;
        }

        public class AnimationDataComponent : Component {
            public float CooldownScale { get; set; }
        }
    }
}