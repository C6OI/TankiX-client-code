using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.Hud.Impl {
    public class ButtonSupplyJoinSystem : ECSSystem {
        [OnEventFire]
        public void SpeedCreated(NodeAddedEvent e, SpeedButtonNode button, [Context] SpeedItemNode item) =>
            AttachEntityToGroup(item.supplyGroup, button.Entity);

        [OnEventFire]
        public void MineCreated(NodeAddedEvent e, MineButtonNode button, [Context] MineItemNode item) =>
            AttachEntityToGroup(item.supplyGroup, button.Entity);

        [OnEventFire]
        public void RepairCreated(NodeAddedEvent e, RepairButtonNode button, [Context] RepairItemNode item) =>
            AttachEntityToGroup(item.supplyGroup, button.Entity);

        [OnEventFire]
        public void ArmorCreated(NodeAddedEvent e, ArmorButtonNode button, [Context] ArmorItemNode item) =>
            AttachEntityToGroup(item.supplyGroup, button.Entity);

        [OnEventFire]
        public void DamageCreated(NodeAddedEvent e, DamageButtonNode button, [Context] DamageItemNode item) =>
            AttachEntityToGroup(item.supplyGroup, button.Entity);

        void AttachEntityToGroup(SupplyGroupComponent group, Entity entity) => group.Attach(entity);

        public class RepairItemNode : Node {
            public RepairInventoryComponent repairInventory;

            public SupplyGroupComponent supplyGroup;
        }

        [Not(typeof(SupplyGroupComponent))]
        public class RepairButtonNode : Node {
            public RepairItemButtonComponent repairItemButton;
        }

        public class ArmorItemNode : Node {
            public ArmorInventoryComponent armorInventory;

            public SupplyGroupComponent supplyGroup;
        }

        [Not(typeof(SupplyGroupComponent))]
        public class ArmorButtonNode : Node {
            public ArmorItemButtonComponent armorItemButton;
        }

        public class DamageItemNode : Node {
            public DamageInventoryComponent damageInventory;

            public SupplyGroupComponent supplyGroup;
        }

        [Not(typeof(SupplyGroupComponent))]
        public class DamageButtonNode : Node {
            public DamageItemButtonComponent damageItemButton;
        }

        public class SpeedItemNode : Node {
            public SpeedInventoryComponent speedInventory;

            public SupplyGroupComponent supplyGroup;
        }

        [Not(typeof(SupplyGroupComponent))]
        public class SpeedButtonNode : Node {
            public SpeedItemButtonComponent speedItemButton;
        }

        public class MineItemNode : Node {
            public MineInventoryComponent mineInventory;

            public SupplyGroupComponent supplyGroup;
        }

        [Not(typeof(SupplyGroupComponent))]
        public class MineButtonNode : Node {
            public MineItemButtonComponent mineItemButton;
        }
    }
}