using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class InventoryActivationSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void CheckRepairActivate(UpdateEvent e, RepairNode node) {
            if (InputManager.CheckAction(InventoryAction.INVENTORY_REPAIR)) {
                ScheduleEvent<SupplyActivationRequestEvent>(node);
            }
        }

        [OnEventFire]
        public void CheckDouble(UpdateEvent e, ArmorNode node) {
            if (InputManager.CheckAction(InventoryAction.INVENTORY_ARMOR)) {
                ScheduleEvent<SupplyActivationRequestEvent>(node);
            }
        }

        [OnEventFire]
        public void CheckDoubleDamageEffectActivate(UpdateEvent e, DamageNode node) {
            if (InputManager.CheckAction(InventoryAction.INVENTORY_DAMAGE)) {
                ScheduleEvent<SupplyActivationRequestEvent>(node);
            }
        }

        [OnEventFire]
        public void CheckSpeedEffectActivate(UpdateEvent e, SpeedNode node) {
            if (InputManager.CheckAction(InventoryAction.INVENTORY_SPEED)) {
                ScheduleEvent<SupplyActivationRequestEvent>(node);
            }
        }

        [OnEventFire]
        public void HandleEffectActivate(NodeAddedEvent e, EffectActivatedNode node) =>
            node.Entity.RemoveComponent<InventoryItemActivateIntentComponent>();

        [OnEventComplete]
        public void CheckMinePut(UpdateEvent e, MineBeforePutNode node, [JoinByUser] SelfTankNode tank) {
            if (InputManager.CheckAction(InventoryAction.INVENTORY_MINE)) {
                Vector3 position = tank.hullInstance.HullInstance.transform.position;
                RaycastHit hitInfo;

                if (Physics.Raycast(position + Vector3.up,
                        Vector3.down,
                        out hitInfo,
                        MineUtil.TANK_MINE_RAYCAST_DISTANCE,
                        LayerMasks.STATIC)) {
                    node.Entity.AddComponent<InventoryItemActivateIntentComponent>();
                    ScheduleEvent<SendTankMovementEvent>(tank);
                    ScheduleEvent<SupplyActivationRequestEvent>(node);
                }
            }
        }

        [OnEventFire]
        public void HandleMinePut(NodeAddedEvent e, MineAfterPutNode node) =>
            node.Entity.RemoveComponent<InventoryItemActivateIntentComponent>();

        [Not(typeof(InventoryItemActivateIntentComponent))]
        public class RepairNode : Node {
            public InventoryItemEnabledStateComponent inventoryItemEnabledState;
            public RepairInventoryComponent repairInventory;
        }

        [Not(typeof(InventoryItemActivateIntentComponent))]
        public class ArmorNode : Node {
            public ArmorInventoryComponent armorInventory;

            public InventoryItemEnabledStateComponent inventoryItemEnabledState;
        }

        [Not(typeof(InventoryItemActivateIntentComponent))]
        public class DamageNode : Node {
            public DamageInventoryComponent damageInventory;

            public InventoryItemEnabledStateComponent inventoryItemEnabledState;
        }

        [Not(typeof(InventoryItemActivateIntentComponent))]
        public class SpeedNode : Node {
            public InventoryItemEnabledStateComponent inventoryItemEnabledState;
            public SpeedInventoryComponent speedInventory;
        }

        public class EffectActivatedNode : Node {
            public InventoryItemActivatedStateComponent inventoryItemActivatedState;

            public InventoryItemActivateIntentComponent inventoryItemActivateIntent;
        }

        [Not(typeof(InventoryItemActivateIntentComponent))]
        public class MineBeforePutNode : Node {
            public InventoryItemEnabledStateComponent inventoryItemEnabledState;

            public MineActivationEnabledComponent mineActivationEnabled;
            public MineInventoryComponent mineInventory;
        }

        public class MineAfterPutNode : Node {
            public InventoryItemActivateIntentComponent inventoryItemActivateIntent;

            public InventoryItemCooldownStateComponent inventoryItemCooldownState;

            public MineActivationEnabledComponent mineActivationEnabled;
            public MineInventoryComponent mineInventory;
        }

        public class SelfTankNode : Node {
            public HullInstanceComponent hullInstance;
            public SelfTankComponent selfTank;
        }
    }
}