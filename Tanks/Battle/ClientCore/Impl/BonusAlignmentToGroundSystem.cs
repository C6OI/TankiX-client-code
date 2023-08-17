using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusAlignmentToGroundSystem : ECSSystem {
        [OnEventFire]
        public void PrepareAlignmentToGround(NodeAddedEvent e, BonusAlignmentToGroundPrepareNode bonus,
            [JoinAll] SingleNode<BonusClientConfigComponent> bonusConfig) {
            Date beginDate = bonus.bonusDropTime.DropTime + bonus.bonusData.FallDuration;
            bonus.Entity.AddComponent<BonusParachuteFoldingStateComponent>();

            if (Date.Now.GetProgress(beginDate, bonus.bonusData.AlignmentToGroundDuration).Equals(1f)) {
                bonus.Entity.RemoveComponent<BonusAlignmentToGroundStateComponent>();
            } else if (bonus.bonusData.GroundPointNormal == Vector3.up) {
                bonus.Entity.RemoveComponent<BonusAlignmentToGroundStateComponent>();
            }
        }

        [OnEventFire]
        public void BoxAlignmentToGround(UpdateEvent e, BonusBoxAlignmentToGroundNode bonus) {
            BonusConfigComponent bonusConfig = bonus.bonusConfig;
            BonusDataComponent bonusData = bonus.bonusData;
            GameObject bonusBoxInstance = bonus.bonusBoxInstance.BonusBoxInstance;
            Transform transform = bonusBoxInstance.transform;

            float num = bonusConfig.AlignmentToGroundAngularSpeed *
                        (Date.Now - bonus.bonusDropTime.DropTime - bonus.bonusData.FallDuration);

            float num2 = Vector3.Angle(transform.TransformDirection(Vector3.up), Vector3.up);
            float num3 = num - num2;
            transform.RotateAround(bonusData.LandingPoint, bonusData.LandingAxis, num3);
            float num4 = Vector3.Angle(bonus.bonusData.GroundPointNormal, transform.TransformDirection(Vector3.up));

            if (num4 <= num3) {
                bonus.Entity.RemoveComponent<BonusAlignmentToGroundStateComponent>();
            }
        }

        [OnEventFire]
        public void BoxAlignmentToGround(NodeRemoveEvent e, BonusBoxAlignmentToGroundNode bonus) {
            BonusDataComponent bonusData = bonus.bonusData;
            GameObject bonusBoxInstance = bonus.bonusBoxInstance.BonusBoxInstance;
            Transform transform = bonusBoxInstance.transform;
            float angle = Vector3.Angle(bonusData.GroundPointNormal, transform.TransformDirection(Vector3.up));
            transform.RotateAround(bonusData.LandingPoint, bonusData.LandingAxis, angle);
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = bonus.rotation.RotationEuler.y + bonusData.FallDuration * bonus.bonusConfig.AngularSpeed;
            transform.eulerAngles = eulerAngles;
        }

        public class BonusAlignmentToGroundPrepareNode : Node {
            public BonusComponent bonus;

            public BonusAlignmentToGroundStateComponent bonusAlignmentToGroundState;

            public BonusDataComponent bonusData;

            public BonusDropTimeComponent bonusDropTime;

            public BonusParachuteInstanceComponent bonusParachuteInstance;

            public TopParachuteMarkerComponent topParachuteMarker;
        }

        public class BonusBoxAlignmentToGroundNode : Node {
            public BonusComponent bonus;

            public BonusAlignmentToGroundStateComponent bonusAlignmentToGroundState;

            public BonusBoxInstanceComponent bonusBoxInstance;

            public BonusConfigComponent bonusConfig;

            public BonusDataComponent bonusData;
            public BonusDropTimeComponent bonusDropTime;

            public RotationComponent rotation;
        }
    }
}