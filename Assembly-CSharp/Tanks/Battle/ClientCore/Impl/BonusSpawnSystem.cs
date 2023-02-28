using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusSpawnSystem : ECSSystem {
        [OnEventFire]
        public void SetBonusPosition(SetBonusPositionEvent e, BonusBoxBaseNode bonus) {
            bonus.bonusBoxInstance.BonusBoxInstance.transform.position = bonus.position.Position;
            bonus.bonusBoxInstance.BonusBoxInstance.transform.rotation = Quaternion.Euler(bonus.rotation.RotationEuler);
        }

        [OnEventFire]
        public void SetBonusPositionOnSpawn(NodeAddedEvent e, BonusBoxSpawnNode bonus) {
            float progress = Date.Now.GetProgress(bonus.bonusDropTime.DropTime, bonus.bonusConfig.SpawnDuration);
            bonus.Entity.AddComponent(new LocalDurationComponent(bonus.bonusConfig.SpawnDuration * (1f - progress)));
            ScheduleEvent<SetBonusPositionEvent>(bonus);
        }

        [OnEventFire]
        public void SetFallingState(NodeAddedEvent e, BonusParachuteSpawnNode bonus) {
            bonus.Entity.AddComponent<BonusFallingStateComponent>();
        }

        [OnEventFire]
        public void BonusOnGroundAnimation(UpdateEvent e, BonusBoxSpawnOnGroundLocalDurationNode bonus) {
            float progress = Date.Now.GetProgress(bonus.localDuration.StartedTime, bonus.localDuration.Duration);
            float num = 0.5f + progress * 0.5f;
            bonus.bonusBoxInstance.BonusBoxInstance.transform.localScale = Vector3.one * num;
        }

        [OnEventFire]
        public void RemoveOnGroundState(LocalDurationExpireEvent e, BonusBoxSpawnOnGroundNode bonus) {
            bonus.bonusBoxInstance.BonusBoxInstance.transform.localScale = Vector3.one;
            bonus.Entity.RemoveComponent<BonusSpawnOnGroundStateComponent>();
        }

        [OnEventComplete]
        public void SetActiveState(LocalDurationExpireEvent e, BonusBoxSpawnNode bonus) {
            bonus.Entity.RemoveComponent<BonusSpawnStateComponent>();
            bonus.Entity.AddComponent<BonusActiveStateComponent>();
        }

        public class BonusBoxBaseNode : Node {
            public BattleGroupComponent battleGroup;

            public BonusComponent bonus;

            public BonusBoxInstanceComponent bonusBoxInstance;
            public BonusConfigComponent bonusConfig;

            public BonusDropTimeComponent bonusDropTime;

            public PositionComponent position;

            public RotationComponent rotation;
        }

        public class BonusBoxSpawnNode : BonusBoxBaseNode {
            public BonusSpawnStateComponent bonusSpawnState;
        }

        public class BonusBoxSpawnOnGroundNode : BonusBoxSpawnNode {
            public BonusSpawnOnGroundStateComponent bonusSpawnOnGroundState;
        }

        public class BonusBoxSpawnOnGroundLocalDurationNode : BonusBoxSpawnOnGroundNode {
            public LocalDurationComponent localDuration;
        }

        public class BonusParachuteSpawnNode : BonusBoxSpawnNode {
            public BonusParachuteInstanceComponent bonusParachuteInstance;

            public LocalDurationComponent localDuration;
        }
    }
}