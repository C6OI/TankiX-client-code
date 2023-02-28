using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ForceFieldSlotActivationValidatorSystem : ECSSystem {
        [OnEventFire]
        public void MarkModuleAsForceField(NodeAddedEvent e, ModuleUserItemNode module, [JoinByMarketItem] [Context] ForceFieldModuleUpgradeInfoNode info) {
            module.Entity.AddComponent<ForceFieldModuleComponent>();
        }

        [OnEventFire]
        public void UpdateActivatePossibility(UpdateEvent e, WeaponNode weaponNode, [JoinByTank] [Combine] SlotNode slot, [JoinByModule] ForceFieldModuleNode module) {
            Transform transform = weaponNode.weaponInstance.WeaponInstance.transform;

            if (ForceFieldTransformUtil.CanFallToTheGround(transform)) {
                EnableActivation(slot.Entity);
            } else {
                DisableActivation(slot.Entity);
            }
        }

        void EnableActivation(Entity inventory) {
            inventory.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        void DisableActivation(Entity inventory) {
            inventory.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        public class SlotNode : Node {
            public ModuleGroupComponent moduleGroup;

            public SlotUserItemInfoComponent slotUserItemInfo;

            public TankGroupComponent tankGroup;
        }

        public class ForceFieldModuleNode : Node {
            public ForceFieldModuleComponent forceFieldModule;

            public ModuleEffectsComponent moduleEffects;
            public ModuleGroupComponent moduleGroup;
        }

        [Not(typeof(ForceFieldModuleComponent))]
        public class ModuleUserItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public ModuleItemComponent moduleItem;

            public UserItemComponent userItem;
        }

        public class ForceFieldModuleUpgradeInfoNode : Node {
            public ForceFieldModuleComponent forceFieldModule;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class WeaponNode : Node {
            public SelfComponent self;
            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;
        }
    }
}