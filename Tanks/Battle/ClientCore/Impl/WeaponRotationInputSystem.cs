using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class WeaponRotationInputSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        float GetMouseXDelta(GameObject weaponInstance, float mouseSpeed, float deltaTime) {
            float num = InputManager.GetAxis(CameraRotationActions.MOUSEX_ROTATE) * mouseSpeed / deltaTime;
            float degrees = Vector3.Angle(weaponInstance.transform.up, Vector3.up);
            degrees = MathUtil.ClampAngle180(degrees);

            if (Mathf.Abs(degrees) > 90f) {
                num *= -1f;
            }

            return num;
        }

        [OnEventFire]
        public void SwitchContextToAiming(NodeAddedEvent evt, SingleNode<ShaftAimingWorkActivationStateComponent> shaftNode,
            [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder,
            [JoinByUser] SingleNode<SelfBattleUserComponent> selfBattleUser) =>
            mouseControlStateHolder.component.MouseControlEnable =
                mouseControlStateHolder.component.MouseControlAllowed &&
                InputManager.GetMouseButton(UnityInputConstants.MOUSE_BUTTON_LEFT);

        [OnEventFire]
        public void OnWeaponUpdateEvent(UpdateEvent e, WeaponNode weapon, [JoinByTank] SelfTankNode tank,
            [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder,
            [JoinAll] Optional<SingleNode<BattleShaftAimingStateComponent>> shaftAimingState) {
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;
            float num = Camera.main.fieldOfView * mouseControlStateHolder.component.MouseSensivity * 1f / Screen.height;

            if (shaftAimingState.IsPresent()) {
                if (mouseControlStateHolder.component.MouseControlEnable) {
                    float num2 = InputManager.GetAxis(CameraRotationActions.MOUSEY_MOVE_SHAFT_AIM);

                    if (mouseControlStateHolder.component.MouseVerticalInverted) {
                        num2 *= -1f;
                    }

                    weaponRotationControl.MouseShaftAimCumulativeVerticalAngle += num2 * num / e.DeltaTime;
                    float mouseXDelta = GetMouseXDelta(weapon.weaponInstance.WeaponInstance, num, e.DeltaTime);

                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle =
                        MathUtil.ClampAngle180(weaponRotationControl.MouseRotationCumulativeHorizontalAngle + mouseXDelta);

                    weaponRotationControl.Centering = false;
                    weaponRotationControl.CenteringControl = false;
                    weaponRotationControl.BlockRotate = InputManager.CheckAction(WeaponActions.WEAPON_LOCK_ROTATION);
                } else {
                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle = 0f;
                    int num3 = InputManager.CheckAction(ShaftAimingActions.AIMING_UP) ? 1 : 0;
                    int num4 = InputManager.CheckAction(ShaftAimingActions.AIMING_DOWN) ? 1 : 0;
                    weaponRotationControl.ShaftElevationDirectionByKeyboard = num3 - num4;
                    float axis = InputManager.GetAxis(WeaponActions.WEAPON_RIGHT);
                    float axis2 = InputManager.GetAxis(WeaponActions.WEAPON_LEFT);
                    weaponRotationControl.Control = Mathf.Clamp(axis - axis2, -1f, 1f);
                    weaponRotationControl.CenteringControl = InputManager.CheckAction(WeaponActions.WEAPON_CENTER);
                }
            } else {
                float axis3 = InputManager.GetAxis(WeaponActions.WEAPON_RIGHT);
                float axis4 = InputManager.GetAxis(WeaponActions.WEAPON_LEFT);
                weaponRotationControl.Control = Mathf.Clamp(axis3 - axis4, -1f, 1f);
                weaponRotationControl.CenteringControl = InputManager.CheckAction(WeaponActions.WEAPON_CENTER);

                if (weaponRotationControl.Control == 0f && !weaponRotationControl.CenteringControl) {
                    float num5 = InputManager.GetAxis(CameraRotationActions.MOUSEY_ROTATE) * num / e.DeltaTime;
                    float mouseXDelta2 = GetMouseXDelta(weapon.weaponInstance.WeaponInstance, num, e.DeltaTime);

                    if (mouseXDelta2 != 0f || num5 != 0f) {
                        mouseControlStateHolder.component.MouseControlEnable = true;
                    }

                    if (mouseControlStateHolder.component.MouseVerticalInverted) {
                        num5 *= -1f;
                    }

                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle =
                        MathUtil.ClampAngle180(weaponRotationControl.MouseRotationCumulativeHorizontalAngle + mouseXDelta2);

                    weaponRotationControl.MouseRotationCumulativeVerticalAngle += num5;
                    weaponRotationControl.BlockRotate = InputManager.CheckAction(WeaponActions.WEAPON_LOCK_ROTATION);
                    weaponRotationControl.CenteringControl = InputManager.CheckAction(WeaponActions.WEAPON_CENTER_BY_MOUSE);
                } else {
                    mouseControlStateHolder.component.MouseControlEnable = false;
                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle = 0f;
                }
            }

            weaponRotationControl.Centering |= weaponRotationControl.CenteringControl;

            if (weaponRotationControl.Centering &&
                (weaponRotationControl.Control != 0f || weaponRotationControl.Rotation == 0f)) {
                weaponRotationControl.Centering = false;
            }
        }

        [OnEventFire]
        public void Deactivate(NodeRemoveEvent e, SelfTankNode tank, [JoinByTank] WeaponNode weapon) =>
            weapon.weaponRotationControl.Control = 0f;

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;

            public TankMovableComponent tankMovable;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;

            public WeaponRotationControlComponent weaponRotationControl;
        }
    }
}