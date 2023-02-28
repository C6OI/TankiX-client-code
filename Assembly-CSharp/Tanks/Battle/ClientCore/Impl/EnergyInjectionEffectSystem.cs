using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class EnergyInjectionEffectSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void ExecuteEnergyInjection(ExecuteEnergyInjectionEvent e, SingleNode<CooldownTimerComponent> weapon) {
            if (!weapon.Entity.HasComponent<MagazineWeaponComponent>()) {
                weapon.component.CooldownTimerSec = 0f;
            }
        }

        [OnEventComplete]
        public void SwitchStreamWeaponToWorking(ExecuteEnergyInjectionEvent e, SingleNode<StreamWeaponComponent> weapon, [JoinSelf] SingleNode<ShootableComponent> node,
            [JoinByTank] SingleNode<TankActiveStateComponent> tank) {
            if (InputManager.CheckAction(ShotActions.SHOT)) {
                StreamWeaponControllerSystem.SwitchIdleModeToWorkingMode(weapon.Entity);
            }
        }

        [OnEventFire]
        public void CheckEnergyInjectionSlot(EarlyUpdateEvent e, StreamWeaponEnergyNode weapon, [JoinByTank] ObservationSlotNode slot,
            [JoinByModule] SingleNode<EnergyInjectionModuleReloadEnergyComponent> module) {
            if (CheckBlock(weapon.weaponEnergy, module.component, slot.Entity)) {
                slot.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
            }
        }

        [OnEventFire]
        public void CheckEnergyInjectionSlot(EarlyUpdateEvent e, DiscreteWeaponEnergyNode weapon, [JoinByTank] ObservationSlotNode slot,
            [JoinByModule] SingleNode<EnergyInjectionModuleReloadEnergyComponent> module) {
            if (CheckBlock(weapon.weaponEnergy, module.component, slot.Entity) && CheckBlock(weapon.cooldownTimer, slot.Entity)) {
                slot.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
            }
        }

        [OnEventFire]
        public void CheckEnergyInjectionSlot(EarlyUpdateEvent e, MagazineNode weapon, [JoinByTank] ObservationSlotNode slot,
            [JoinByModule] SingleNode<EnergyInjectionModuleReloadEnergyComponent> module) {
            if (CheckBlock(weapon.cooldownTimer, slot.Entity)) {
                if (weapon.magazineStorage.CurrentCartridgeCount < weapon.magazineWeapon.MaxCartridgeCount) {
                    slot.Entity.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
                } else {
                    slot.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
                }
            }
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, SpawnTankNode tank, [JoinByTank] [Context] SlotNode slot, [JoinByModule] [Context] ModuleNode module) {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, SemiActiveTankNode tank, [JoinByTank] [Context] SlotNode slot, [JoinByModule] [Context] ModuleNode module) {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, ActiveTankNode tank, [JoinByTank] [Context] SlotNode slot, [JoinByModule] [Context] ModuleNode module) {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, DeadTankNode tank, [JoinByTank] [Context] SlotNode slot, [JoinByModule] [Context] ModuleNode module) {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, NewTankNode tank, [JoinByTank] [Context] SlotNode slot, [JoinByModule] [Context] ModuleNode module) {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, SingleNode<EnergyInjectionSlotStopObservationComponent> slot) {
            slot.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        [OnEventComplete]
        public void CleanStopObservation(UpdateEvent e, SingleNode<EnergyInjectionSlotStopObservationComponent> slot, [JoinByTank] ActiveTankNode tank) {
            slot.Entity.RemoveComponent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void CleanStopObservation(NodeRemoveEvent e, SingleNode<TankGroupComponent> node, [JoinSelf] SingleNode<EnergyInjectionSlotStopObservationComponent> slot) {
            slot.Entity.RemoveComponent<EnergyInjectionSlotStopObservationComponent>();
        }

        bool CheckBlock(WeaponEnergyComponent weaponEnergyComponent, EnergyInjectionModuleReloadEnergyComponent energyInjectionModuleReloadEnergyComponent, Entity slot) {
            if (weaponEnergyComponent.Energy < energyInjectionModuleReloadEnergyComponent.ReloadEnergyPercent) {
                slot.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
                return false;
            }

            return true;
        }

        bool CheckBlock(CooldownTimerComponent cooldownTimerComponent, Entity slot) {
            if (cooldownTimerComponent.CooldownTimerSec > 0f) {
                slot.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
                return false;
            }

            return true;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class WeaponEnergyNode : WeaponNode {
            public CooldownTimerComponent cooldownTimer;
            public WeaponEnergyComponent weaponEnergy;
        }

        public class DiscreteWeaponEnergyNode : WeaponEnergyNode {
            public DiscreteWeaponComponent discreteWeapon;
        }

        public class StreamWeaponEnergyNode : WeaponEnergyNode {
            public StreamWeaponComponent streamWeapon;
        }

        public class MagazineNode : WeaponNode {
            public CooldownTimerComponent cooldownTimer;

            public MagazineStorageComponent magazineStorage;

            public MagazineWeaponComponent magazineWeapon;
        }

        public class ModuleNode : Node {
            public EnergyInjectionModuleReloadEnergyComponent energyInjectionModuleReloadEnergy;
            public ModuleGroupComponent moduleGroup;
        }

        public class SlotNode : Node {
            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;

            public TankGroupComponent tankGroup;
        }

        [Not(typeof(EnergyInjectionSlotStopObservationComponent))]
        public class ObservationSlotNode : SlotNode { }

        public class TankNode : Node {
            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        public class NewTankNode : TankNode {
            public TankNewStateComponent tankNewState;
        }

        public class DeadTankNode : TankNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class SpawnTankNode : TankNode {
            public TankSpawnStateComponent tankSpawnState;
        }

        public class SemiActiveTankNode : TankNode {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class ActiveTankNode : TankNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class EnergyInjectionSlotStopObservationComponent : Component { }
    }
}