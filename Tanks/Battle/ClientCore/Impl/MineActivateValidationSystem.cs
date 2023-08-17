using System.Collections.Generic;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MineActivateValidationSystem : ECSSystem {
        [OnEventFire]
        public void DMActivation(NodeAddedEvent e, MineInventoryNode mineInventory) =>
            EnableActivation(mineInventory.Entity);

        [OnEventFire]
        public void CTFActivation(UpdateEvent e, MineInventoryNode mineInventory, [JoinAll] SelfTankNode tank,
            [JoinAll] ICollection<SingleNode<FlagPedestalComponent>> flagPedestals, [JoinAll] CTFBattleNode battle) {
            Vector3 position = tank.hullInstance.HullInstance.transform.position;

            if (HasActivationMine(position, flagPedestals, battle)) {
                EnableActivation(mineInventory.Entity);
            } else {
                DisableActivation(mineInventory.Entity);
            }
        }

        bool HasActivationMine(Vector3 tankPosition, ICollection<SingleNode<FlagPedestalComponent>> flagPedestals,
            CTFBattleNode battle) {
            RaycastHit hitInfo;

            if (!Physics.Raycast(tankPosition + Vector3.up,
                    Vector3.down,
                    out hitInfo,
                    MineUtil.TANK_MINE_RAYCAST_DISTANCE,
                    LayerMasks.STATIC)) {
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
            if (!inventory.HasComponent<MineActivationEnabledComponent>()) {
                inventory.AddComponent<MineActivationEnabledComponent>();
            }
        }

        void DisableActivation(Entity inventory) {
            if (inventory.HasComponent<MineActivationEnabledComponent>()) {
                inventory.RemoveComponent<MineActivationEnabledComponent>();
            }
        }

        public class SelfTankNode : Node {
            public BattleGroupComponent battleGroup;

            public HullInstanceComponent hullInstance;

            public SelfTankComponent selfTank;
        }

        public class MineInventoryNode : Node {
            public BattleGroupComponent battleGroup;

            public MineInventoryComponent mineInventory;
        }

        public class CTFBattleNode : Node {
            public CTFComponent ctf;

            public CTFConfigComponent ctfConfig;
            public SelfComponent self;
        }
    }
}