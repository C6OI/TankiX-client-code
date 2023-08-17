using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusSpawnSystem : ECSSystem {
        [OnEventFire]
        public void SetBonusPositionOnSpawn(NodeAddedEvent e, BonusSpawnNode bonus) {
            float progress = Date.Now.GetProgress(bonus.bonusDropTime.DropTime, bonus.bonusConfig.SpawnDuration);
            bonus.Entity.AddComponent(new LocalDurationComponent(bonus.bonusConfig.SpawnDuration * (1f - progress)));
            ScheduleEvent<SetBonusPositionEvent>(bonus);
        }

        [OnEventFire]
        public void SetBonusPosition(SetBonusPositionEvent e, BonusSpawnNode bonus) {
            bonus.bonusBoxInstance.BonusBoxInstance.transform.position = bonus.position.Position;
            bonus.bonusBoxInstance.BonusBoxInstance.transform.rotation = Quaternion.Euler(bonus.rotation.RotationEuler);
        }

        [OnEventFire]
        public void SetFallingState(NodeAddedEvent e, BonusParachuteSpawnNode bonus) =>
            bonus.Entity.AddComponent<BonusFallingStateComponent>();

        [OnEventFire]
        public void BonusOnGroundAnimation(UpdateEvent e, BonusOnGroundSpawnNode bonus) {
            float progress = Date.Now.GetProgress(bonus.localDuration.StartedTime, bonus.localDuration.Duration);
            float num = 0.5f + progress * 0.5f;
            bonus.bonusBoxInstance.BonusBoxInstance.transform.localScale = Vector3.one * num;
        }

        [OnEventFire]
        public void SetActiveState(LocalDurationExpireEvent e, BonusSpawnNode bonus) {
            bonus.Entity.RemoveComponent<BonusSpawnStateComponent>();
            bonus.Entity.AddComponent<BonusActiveStateComponent>();
        }

        [OnEventFire]
        public void RemoveOnGroundState(LocalDurationExpireEvent e, BonusOnGroundSpawnNode bonus) {
            bonus.bonusBoxInstance.BonusBoxInstance.transform.localScale = Vector3.one;
            bonus.Entity.RemoveComponent<BonusSpawnOnGroundStateComponent>();
        }

        public class BonusSpawnNode : Node {
            public BonusBoxInstanceComponent bonusBoxInstance;

            public BonusConfigComponent bonusConfig;

            public BonusDropTimeComponent bonusDropTime;
            public BonusSpawnStateComponent bonusSpawnState;

            public PositionComponent position;

            public RotationComponent rotation;
        }

        public class BonusOnGroundSpawnNode : Node {
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BonusSpawnOnGroundStateComponent bonusSpawnOnGroundState;

            public LocalDurationComponent localDuration;
        }

        public class BonusParachuteSpawnNode : Node {
            public BonusComponent bonus;

            public BonusBoxInstanceComponent bonusBoxInstance;

            public BonusConfigComponent bonusConfig;

            public BonusDropTimeComponent bonusDropTime;

            public BonusParachuteInstanceComponent bonusParachuteInstance;

            public BonusSpawnStateComponent bonusSpawnState;

            public LocalDurationComponent localDuration;

            public PositionComponent position;

            public RotationComponent rotation;
        }
    }
}