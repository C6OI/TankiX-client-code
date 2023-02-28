using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MineSystem : ECSSystem {
        const float MINE_HALF_SIZE = 0.5f;

        [OnEventFire]
        public void PrepareMinePosition(NodeAddedEvent evt, [Combine] MineNode mine, SingleNode<MapInstanceComponent> map) {
            NewEvent(new InitMinePlacingTransformEvent(mine.minePosition.Position)).AttachAll(mine, map).Schedule();
        }

        [OnEventFire]
        public void PlaceMineOnGround(NodeAddedEvent e, MineInstanceNode mine) {
            MinePlacingTransformComponent minePlacingTransform = mine.minePlacingTransform;
            Transform transform = mine.effectInstance.GameObject.transform;

            if (mine.minePlacingTransform.HasPlacingTransform) {
                transform.SetPositionSafe(minePlacingTransform.PlacingData.point);
                transform.SetRotationSafe(Quaternion.FromToRotation(Vector3.up, minePlacingTransform.PlacingData.normal));
            } else {
                transform.SetPositionSafe(mine.minePosition.Position);
            }
        }

        [OnEventFire]
        public void ActivateMineTrigger(NodeAddedEvent e, ActiveMineNode mine, [JoinByTank] [Context] EnemyTankNode tank) {
            GameObject gameObject = mine.effectInstance.GameObject;
            Rigidbody componentInChildren = gameObject.GetComponentInChildren<Rigidbody>();
            MeshCollider componentInChildren2 = componentInChildren.GetComponentInChildren<MeshCollider>();
            float num = 1f;
            float num2 = (mine.mineEffectTriggeringArea.Radius + 0.5f) * 2f;
            Vector3 localScale = componentInChildren2.transform.localScale;
            float x = localScale.x;
            float num3 = x * num2 / num;
            componentInChildren2.transform.localScale = new Vector3(num3, localScale.y, num3);
            MinePhysicsTriggerBehaviour minePhysicsTriggerBehaviour = componentInChildren.gameObject.AddComponent<MinePhysicsTriggerBehaviour>();
            minePhysicsTriggerBehaviour.TriggerEntity = mine.Entity;
        }

        [OnEventFire]
        public void TriggerMine(TriggerEnterEvent e, ActiveMineNode mine, SingleNode<TankActiveStateComponent> tank) {
            ScheduleEvent<SendTankMovementEvent>(tank);
        }

        public class MineNode : Node {
            public MineConfigComponent mineConfig;
            public MineEffectComponent mineEffect;

            public MineEffectTriggeringAreaComponent mineEffectTriggeringArea;

            public MinePositionComponent minePosition;
        }

        public class MinePlacingTransformNode : MineNode {
            public MinePlacingTransformComponent minePlacingTransform;
        }

        public class MineInstanceNode : MinePlacingTransformNode {
            public EffectInstanceComponent effectInstance;

            public TankGroupComponent tankGroup;
        }

        public class ActiveMineNode : MineInstanceNode {
            public EffectActiveComponent effectActive;
        }

        public class EnemyTankNode : Node {
            public EnemyComponent enemy;

            public TankComponent tank;
            public TankGroupComponent tankGroup;
        }
    }
}