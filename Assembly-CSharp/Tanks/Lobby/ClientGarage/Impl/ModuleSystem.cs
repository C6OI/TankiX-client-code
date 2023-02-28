using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleSystem : ECSSystem {
        [OnEventFire]
        public void BuildModuleUpgradePropertiesInfo(NodeAddedEvent e, ModuleMarketItemUpgradeInfoNode moduleMarketItem) {
            Entity entity = CreateEntity(moduleMarketItem.moduleUpgradePropertiesInfo.Template.TemplateId, moduleMarketItem.moduleUpgradePropertiesInfo.Path);
            moduleMarketItem.marketItemGroup.Attach(entity);
        }

        [OnEventFire]
        public void OnModuleUpgraded(ModuleUpgradedEvent e, ModuleNode node) {
            ScheduleEvent<ModuleChangedEvent>(node);
        }

        [OnEventFire]
        public void OnModuleAssembled(ModuleAssembledEvent e, ModuleNode node) {
            ScheduleEvent<ModuleChangedEvent>(node);
        }

        [OnEventComplete]
        public void OnMountModule(NodeAddedEvent e, MountedModuleNode item, [JoinByUser] SingleNode<SelfUserComponent> user) {
            ScheduleEvent<ModuleChangedEvent>(item);
        }

        [OnEventComplete]
        public void OnUnmountModule(NodeRemoveEvent e, SlotNode slot, [Context] [JoinByModule] ModuleNode item, [JoinByUser] SingleNode<SelfUserComponent> user) {
            ScheduleEvent<ModuleChangedEvent>(item);
        }

        public class ModuleMarketItemUpgradeInfoNode : Node {
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;

            public ModuleItemComponent moduleItem;

            public ModuleUpgradePropertiesInfoComponent moduleUpgradePropertiesInfo;
        }

        public class ModuleNode : Node {
            public ModuleItemComponent moduleItem;
            public SelfComponent self;

            public UserItemComponent userItem;
        }

        public class MountedModuleNode : ModuleNode {
            public MountedItemComponent mountedItem;
        }

        public class SlotNode : Node {
            public SlotTankPartComponent slotTankPart;

            public SlotUserItemInfoComponent slotUserItemInfo;
            public UserItemComponent userItem;
        }
    }
}