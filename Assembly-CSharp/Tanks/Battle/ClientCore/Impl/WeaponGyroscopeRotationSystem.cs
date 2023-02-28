using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class WeaponGyroscopeRotationSystem : ECSSystem {
        [OnEventFire]
        public void TakeOrientationWeapon(NodeAddedEvent evt, WeaponGyroscopeNode weaponGyroscope, [Context] [JoinByTank] TankNode tank) {
            UpdateGyroscopeData(weaponGyroscope, tank);
        }

        [OnEventComplete]
        public void RotateWeapon(WeaponRotationUpdateGyroscopeEvent e, WeaponGyroscopeNode weapon, [JoinByTank] TankNode tank,
            [JoinByTank] Optional<SingleNode<VulcanWeaponComponent>> vulkanWeapon, [JoinAll] SelfBattleUser selfBattleUser) {
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;
            float gyroscopePower = weapon.weaponGyroscopeRotation.GyroscopePower;
            weaponRotationControl.ForceGyroscopeEnabled = vulkanWeapon.IsPresent() && gyroscopePower > float.Epsilon;
            bool mouseControlEnable = selfBattleUser.mouseControlStateHolder.MouseControlEnable;
            bool flag = weaponRotationControl.ForceGyroscopeEnabled || mouseControlEnable;
            WeaponGyroscopeRotationComponent weaponGyroscopeRotation = weapon.weaponGyroscopeRotation;
            weaponGyroscopeRotation.WeaponTurnCoeff = 1f;
            weaponGyroscopeRotation.DeltaAngleOfHullRotation = 0f;
            Vector3 forwardDir = weaponGyroscopeRotation.ForwardDir;
            Vector3 upDir = weaponGyroscopeRotation.UpDir;
            UpdateGyroscopeData(weapon, tank);
            Vector3 forwardDir2 = weaponGyroscopeRotation.ForwardDir;
            Vector3 upDir2 = weaponGyroscopeRotation.UpDir;

            if (!flag) {
                return;
            }

            weaponGyroscopeRotation.DeltaAngleOfHullRotation = CalculateDeltaAngleOfHullRotationAroundUpAxis(forwardDir, forwardDir2, upDir, upDir2);
            float num = tank.speed.TurnSpeed * e.DeltaTime;
            weaponGyroscopeRotation.DeltaAngleOfHullRotation = Mathf.Clamp(weaponGyroscopeRotation.DeltaAngleOfHullRotation, 0f - num, num);

            if (weaponRotationControl.ForceGyroscopeEnabled) {
                if (mouseControlEnable) {
                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle -= weaponGyroscopeRotation.DeltaAngleOfHullRotation * (1f - gyroscopePower);
                }

                float weaponTurnDecelerationCoeff = vulkanWeapon.Get().component.WeaponTurnDecelerationCoeff;
                weaponGyroscopeRotation.WeaponTurnCoeff = weaponTurnDecelerationCoeff + (1f - gyroscopePower) * (1f - weaponTurnDecelerationCoeff);
                weaponGyroscopeRotation.DeltaAngleOfHullRotation *= gyroscopePower;
            }
        }

        void UpdateGyroscopeData(WeaponGyroscopeNode weaponGyroscope, TankNode tank) {
            WeaponGyroscopeRotationComponent weaponGyroscopeRotation = weaponGyroscope.weaponGyroscopeRotation;
            Transform transform = tank.hullInstance.HullInstance.transform;
            weaponGyroscopeRotation.ForwardDir = transform.forward;
            weaponGyroscopeRotation.UpDir = transform.up;
        }

        float CalculateDeltaAngleOfHullRotationAroundUpAxis(Vector3 prevForward, Vector3 nextForward, Vector3 prevUp, Vector3 nextUp) {
            if (prevForward == nextForward && prevUp == nextUp) {
                return 0f;
            }

            Quaternion quaternion = Quaternion.FromToRotation(prevUp, nextUp);
            Vector3 vector = quaternion * prevForward;
            float num = Vector3.Angle(nextForward, vector);
            Vector3 rhs = Vector3.Cross(nextUp, vector);
            float num2 = Mathf.Sign(Vector3.Dot(nextForward, rhs));
            return num2 * num;
        }

        public class TankNode : Node {
            public HullInstanceComponent hullInstance;

            public SpeedComponent speed;
            public TankGroupComponent tankGroup;

            public TankMovableComponent tankMovable;
        }

        public class WeaponGyroscopeNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponGyroscopeRotationComponent weaponGyroscopeRotation;

            public WeaponInstanceComponent weaponInstance;

            public WeaponRotationComponent weaponRotation;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class SelfBattleUser : Node {
            public MouseControlStateHolderComponent mouseControlStateHolder;

            public SelfBattleUserComponent selfBattleUser;
        }
    }
}