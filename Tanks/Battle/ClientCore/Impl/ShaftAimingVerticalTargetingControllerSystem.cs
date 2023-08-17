using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftAimingVerticalTargetingControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void UpdateVerticalTargetingAngle(WeaponRotationUpdateVerticalEvent e,
            AimingVerticalTargetingControllerNode weapon,
            [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder) {
            float num = 0f - weapon.verticalSectorsTargeting.VAngleDown;
            float vAngleUp = weapon.verticalSectorsTargeting.VAngleUp;
            float maxVerticalSpeed = weapon.shaftAimingSpeed.MaxVerticalSpeed;
            float verticalAcceleration = weapon.shaftAimingSpeed.VerticalAcceleration;
            int num2 = weapon.weaponRotationControl.ShaftElevationDirectionByKeyboard;
            bool mouseControlEnable = mouseControlStateHolder.component.MouseControlEnable;

            if (mouseControlEnable) {
                num2 = (int)Mathf.Sign(weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle);

                weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle = Mathf.Clamp(
                    weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle,
                    num - weapon.shaftAimingWorkingState.VerticalAngle,
                    vAngleUp - weapon.shaftAimingWorkingState.VerticalAngle);
            } else {
                weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle = 0f;
            }

            bool isInputActive = false;

            if (weapon.shaftAimingWorkingState.VerticalElevationDir != num2) {
                weapon.shaftAimingWorkingState.VerticalSpeed = 0f;
                weapon.shaftAimingWorkingState.VerticalElevationDir = num2;
                isInputActive = num2 != 0;
            } else {
                weapon.shaftAimingWorkingState.VerticalElevationDir = num2;
                weapon.shaftAimingWorkingState.VerticalSpeed += verticalAcceleration * e.DeltaTime;

                weapon.shaftAimingWorkingState.VerticalSpeed =
                    Mathf.Clamp(weapon.shaftAimingWorkingState.VerticalSpeed, 0f, maxVerticalSpeed);

                float num3 = num2 * weapon.shaftAimingWorkingState.VerticalSpeed * e.DeltaTime;

                if (mouseControlEnable) {
                    if (!weapon.weaponRotationControl.BlockRotate) {
                        num3 = num2 <= 0
                                   ? Mathf.Clamp(num3, weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle, 0f)
                                   : Mathf.Clamp(num3,
                                       0f,
                                       weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle);

                        isInputActive = num3 != 0f;
                        weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle -= num3;
                        weapon.shaftAimingWorkingState.VerticalAngle += num3;

                        weapon.shaftAimingWorkingState.VerticalAngle =
                            Mathf.Clamp(weapon.shaftAimingWorkingState.VerticalAngle, num, vAngleUp);
                    }
                } else {
                    isInputActive = num3 != 0f;
                    weapon.shaftAimingWorkingState.VerticalAngle += num3;

                    weapon.shaftAimingWorkingState.VerticalAngle =
                        Mathf.Clamp(weapon.shaftAimingWorkingState.VerticalAngle, num, vAngleUp);
                }
            }

            MuzzleLogicAccessor muzzleLogicAccessor = new(weapon.muzzlePoint, weapon.weaponInstance);

            weapon.shaftAimingWorkingState.WorkingDirection =
                Quaternion.AngleAxis(weapon.shaftAimingWorkingState.VerticalAngle,
                    muzzleLogicAccessor.GetLeftDirectionWorld()) *
                muzzleLogicAccessor.GetFireDirectionWorld();

            weapon.shaftAimingWorkingState.IsActive = CheckManualTargetingActivity(weapon.weaponRotationControl,
                weapon.verticalSectorsTargeting,
                weapon.shaftAimingWorkingState,
                isInputActive);
        }

        bool CheckManualTargetingActivity(WeaponRotationControlComponent rotation,
            VerticalSectorsTargetingComponent verticalSectors, ShaftAimingWorkingStateComponent working,
            bool isInputActive) {
            if (rotation.IsRotating()) {
                return true;
            }

            if (!isInputActive) {
                return false;
            }

            float verticalAngle = working.VerticalAngle;

            if (verticalAngle == verticalSectors.VAngleUp) {
                return false;
            }

            if (verticalAngle == 0f - verticalSectors.VAngleDown) {
                return false;
            }

            return true;
        }

        public class AimingVerticalTargetingControllerNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public ShaftAimingSpeedComponent shaftAimingSpeed;

            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public VerticalSectorsTargetingComponent verticalSectorsTargeting;

            public WeaponInstanceComponent weaponInstance;

            public WeaponRotationControlComponent weaponRotationControl;
        }
    }
}