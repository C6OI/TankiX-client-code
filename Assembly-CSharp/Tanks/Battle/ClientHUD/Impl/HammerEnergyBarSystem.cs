using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HammerEnergyBarSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void AddAnimationData(NodeAddedEvent e, HammerReloadEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank) {
            if (!weapon.Entity.HasComponent<ReloadAnimationDataComponent>()) {
                weapon.Entity.AddComponent<ReloadAnimationDataComponent>();
            }

            weapon.Entity.GetComponent<ReloadAnimationDataComponent>().CurrentDuration = 0f;
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, HammerEnergyNode weapon, [JoinByTank] [Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud) {
            hud.component.EnergyBarEnabled = true;
            hud.component.MaxEnergyValue = weapon.magazineWeapon.MaxCartridgeCount;
            hud.component.EnergyAmountPerSegment = 1f;
            hud.component.CurrentEnergyValue = 0f;
        }

        [OnEventComplete]
        public void Init(NodeAddedEvent e, HammerReadyEnergyNode weapon, [JoinByTank] [Context] HUDNodes.ActiveSelfTankNode tank, SingleNode<MainHUDComponent> hud) {
            SetMagazineAsReady(weapon, hud);
        }

        [OnEventComplete]
        public void Init(SetMagazineReadyEvent e, HammerReadyEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            SetMagazineAsReady(weapon, hud);
        }

        void SetMagazineAsReady(HammerReadyEnergyNode weapon, SingleNode<MainHUDComponent> hud) {
            hud.component.CurrentEnergyValue = weapon.magazineWeapon.MaxCartridgeCount;

            if (weapon.Entity.HasComponent<ReloadAnimationDataComponent>()) {
                weapon.Entity.RemoveComponent<ReloadAnimationDataComponent>();
            }

            hud.component.EnergyBlink(true);
        }

        [OnEventFire]
        public void UpdateOnTrigger(BaseShotEvent evt, HammerReadyEnergyNode hammerEnergy, [JoinByTank] HUDNodes.ActiveSelfTankNode selfNode, [JoinAll] SingleNode<MainHUDComponent> hud) {
            float currentEnergyValue = hammerEnergy.magazineStorage.CurrentCartridgeCount - 1;
            hud.component.CurrentEnergyValue = currentEnergyValue;
        }

        [OnEventFire]
        public void Reload(TimeUpdateEvent e, HammerEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            if ((weapon.cooldownTimer.CooldownTimerSec > 0f || weapon.Entity.HasComponent<MagazineReloadStateComponent>() || !weapon.Entity.HasComponent<ShootableComponent>()) &&
                InputManager.GetActionKeyDown(ShotActions.SHOT)) {
                hud.component.EnergyBlink(false);
            }
        }

        [OnEventFire]
        public void Reload(TimeUpdateEvent e, HammerReloadingEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            float reloadMagazineTimePerSec = weapon.magazineWeapon.ReloadMagazineTimePerSec;
            int maxCartridgeCount = weapon.magazineWeapon.MaxCartridgeCount;
            float num = reloadMagazineTimePerSec / maxCartridgeCount;
            float currentDuration = weapon.reloadAnimationData.CurrentDuration;
            int num2 = (int)(currentDuration / num);
            hud.component.CurrentEnergyValue = EaseInQuad((currentDuration - num2 * num) / num, num2, 1f);
            weapon.reloadAnimationData.CurrentDuration += e.DeltaTime;
        }

        float EaseInQuad(float time, float from, float delta) => delta * time * time + from;

        public class HammerEnergyNode : Node {
            public CooldownTimerComponent cooldownTimer;

            public HammerComponent hammer;

            public HammerEnergyBarComponent hammerEnergyBar;

            public MagazineStorageComponent magazineStorage;

            public MagazineWeaponComponent magazineWeapon;

            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class HammerReadyEnergyNode : HammerEnergyNode {
            public MagazineReadyStateComponent magazineReadyState;
        }

        public class HammerReloadEnergyNode : HammerEnergyNode {
            public MagazineReloadStateComponent magazineReloadState;
        }

        public class HammerReloadingEnergyNode : HammerReloadEnergyNode {
            public ReloadAnimationDataComponent reloadAnimationData;
        }

        public class ReloadAnimationDataComponent : Component {
            public float CurrentDuration { get; set; }
        }
    }
}