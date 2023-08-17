using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MineSystem : ECSSystem {
        static readonly float TANK_MINE_RAYCAST_DISTANCE = 10000f;

        static readonly float MINE_UNITYOBJECT_DELETION_DELAY = 1.5f;

        [OnEventFire]
        public void PrepareMinePosition(NodeAddedEvent evt, [Combine] MinePositionNode mine,
            [Context] [JoinAll] SingleNode<MapInstanceComponent> map) {
            MinePositionComponent minePosition = mine.minePosition;
            MinePlacingTransformComponent minePlacingTransformComponent = new();
            RaycastHit hitInfo;

            if (Physics.Raycast(minePosition.Position + Vector3.up,
                    Vector3.down,
                    out hitInfo,
                    TANK_MINE_RAYCAST_DISTANCE,
                    LayerMasks.STATIC)) {
                minePlacingTransformComponent.PlacingData = hitInfo;
                minePlacingTransformComponent.HasPlacingTransform = true;
            } else {
                minePlacingTransformComponent.HasPlacingTransform = false;
            }

            mine.Entity.AddComponent(minePlacingTransformComponent);
        }

        [OnEventFire]
        public void Instantiate(NodeAddedEvent e, MinePrefabNode mine) {
            GameObject gameObject = (GameObject)Object.Instantiate(mine.resourceData.Data);
            mine.Entity.AddComponent(new MineInstanceComponent(gameObject));
            PlaceMineOnGround(mine.minePosition, mine.minePlacingTransform, gameObject.transform);
            gameObject.GetComponent<EntityBehaviour>().BuildEntity(mine.Entity);
        }

        void PlaceMineOnGround(MinePositionComponent minePosition, MinePlacingTransformComponent minePlacingTransform,
            Transform mineTransfrom) {
            if (minePlacingTransform.HasPlacingTransform) {
                mineTransfrom.position = minePlacingTransform.PlacingData.point;
                mineTransfrom.rotation = Quaternion.FromToRotation(Vector3.up, minePlacingTransform.PlacingData.normal);
            } else {
                mineTransfrom.position = minePosition.Position;
            }
        }

        [OnEventFire]
        public void Destroy(NodeRemoveEvent e, MineInstanceNode mine) {
            ScheduleEvent<PrepareDestroyMineEvent>(mine);
            GameObject gameObject = mine.mineInstance.GameObject;
            gameObject.AddComponent<DelayedSelfDestroyBehaviour>().Delay = MINE_UNITYOBJECT_DELETION_DELAY;
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        [OnEventFire]
        public void ActivateMineTrigger(NodeAddedEvent e, ActiveEnemyMineNode mine) {
            MinePhysicsTriggerBehaviour minePhysicsTriggerBehaviour =
                mine.mineInstance.GameObject.AddComponent<MinePhysicsTriggerBehaviour>();

            minePhysicsTriggerBehaviour.TriggerEntity = mine.Entity;
        }

        [OnEventFire]
        public void TriggerMine(TriggerEnterEvent e, ActiveEnemyMineNode mine, SingleNode<TankActiveStateComponent> tank) =>
            ScheduleEvent<SendTankMovementEvent>(tank);

        public class MinePrefabNode : Node {
            public MineComponent mine;

            public MinePlacingTransformComponent minePlacingTransform;

            public MinePositionComponent minePosition;

            public ResourceDataComponent resourceData;
        }

        public class MineInstanceNode : Node {
            public MineComponent mine;

            public MineConfigComponent mineConfig;

            public MineInstanceComponent mineInstance;
            public UserGroupComponent userGroup;
        }

        public class ActiveEnemyMineNode : Node {
            public EnemyComponent enemy;
            public MineComponent mine;

            public MineActiveComponent mineActive;

            public MineInstanceComponent mineInstance;
        }

        [Not(typeof(MinePlacingTransformComponent))]
        public class MinePositionNode : Node {
            public MinePositionComponent minePosition;
        }
    }
}