using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MineActivateValidationSystem : ECSSystem {
        [OnEventFire]
        public void DMActivation(NodeAddedEvent e, SelfTankNode tank, [Context] [JoinByTank] [Combine] MineSlotNode slot, [Context] [JoinByModule] MineModuleNode module) {
            EnableActivation(slot.Entity);
        }

        [OnEventFire]
        public void CTFActivation(UpdateEvent e, SelfTankNode tank, [JoinByTank] [Combine] MineSlotNode slot, [JoinByModule] MineModuleNode module,
            [JoinAll] ICollection<SingleNode<FlagPedestalComponent>> flagPedestals, [JoinAll] CTFBattleNode battle) {
            Vector3 position = tank.hullInstance.HullInstance.transform.position;

            if (HasActivationMine(position, flagPedestals, battle)) {
                EnableActivation(slot.Entity);
            } else {
                DisableActivation(slot.Entity);
            }
        }

        bool HasActivationMine(Vector3 tankPosition, ICollection<SingleNode<FlagPedestalComponent>> flagPedestals, CTFBattleNode battle) {
            RaycastHit hitInfo;

            if (!Physics.Raycast(tankPosition + Vector3.up, Vector3.down, out hitInfo, MineUtil.TANK_MINE_RAYCAST_DISTANCE, LayerMasks.STATIC)) {
                return false;
            }

            foreach (SingleNode<FlagPedestalComponent> flagPedestal in flagPedestals) {
                Vector3 position = flagPedestal.component.Position;

                if ((position - hitInfo.point).magnitude < battle.ctfConfig.minDistanceFromMineToBase) {
                    return false;
                }
            }

            return true;
        }

        void EnableActivation(Entity inventory) {
            inventory.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        void DisableActivation(Entity inventory) {
            inventory.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        public class SelfTankNode : Node {
            public BattleGroupComponent battleGroup;

            public HullInstanceComponent hullInstance;

            public SelfTankComponent selfTank;

            public TankGroupComponent tankGroup;
        }

        public class MineModuleNode : Node {
            public ModuleGroupComponent moduleGroup;

            public StaticMineModuleComponent staticMineModule;
        }

        public class MineSlotNode : Node {
            public ModuleGroupComponent moduleGroup;

            public SlotUserItemInfoComponent slotUserItemInfo;

            public TankGroupComponent tankGroup;
        }

        public class CTFBattleNode : Node {
            public CTFComponent ctf;

            public CTFConfigComponent ctfConfig;
            public SelfComponent self;
        }
    }
}