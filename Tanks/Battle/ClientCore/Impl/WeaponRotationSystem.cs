using System;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class WeaponRotationSystem : ECSSystem {
        [OnEventFire]
        public void OnTankInactive(NodeRemoveEvent e, SelfTankNode selfTank, [JoinByTank] WeaponNode weapon) {
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;

            if (weaponRotationControl.EffectiveControl != 0f) {
                weaponRotationControl.PrevEffectiveControl = 0f;
                weaponRotationControl.EffectiveControl = 0f;
                ScheduleEvent(BaseWeaponRotationUpdateEvent<WeaponRotationControlChangedEvent>.Instance, weapon);
            }
        }

        [OnEventFire]
        public void UpdateSelfCommonWeapon(TimeUpdateEvent e, SimpleWeaponNode weapon, [JoinByTank] SelfTankNode tank,
            [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder) {
            float deltaTime = e.DeltaTime;

            ScheduleEvent(BaseWeaponRotationUpdateDeltaTimeEvent<WeaponRotationUpdateGyroscopeEvent>.GetInstance(deltaTime),
                weapon);

            UpdateWeaponRotation(weapon, mouseControlStateHolder, deltaTime);
        }

        [OnEventFire]
        public void UpdateSelfShaftWeapon(TimeUpdateEvent e, ShaftWeaponNode weapon, [JoinByTank] SelfTankNode tank,
            [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder) {
            float deltaTime = e.DeltaTime;

            ScheduleEvent(BaseWeaponRotationUpdateDeltaTimeEvent<WeaponRotationUpdateGyroscopeEvent>.GetInstance(deltaTime),
                weapon);

            UpdateWeaponRotation(weapon, mouseControlStateHolder, deltaTime);

            ScheduleEvent(BaseWeaponRotationUpdateDeltaTimeEvent<WeaponRotationUpdateVerticalEvent>.GetInstance(deltaTime),
                weapon);

            ScheduleEvent(
                BaseWeaponRotationUpdateDeltaTimeEvent<WeaponRotationUpdateShaftAimingCameraEvent>.GetInstance(deltaTime),
                weapon);

            ScheduleEvent(BaseWeaponRotationUpdateEvent<WeaponRotationUpdateAimEvent>.Instance, weapon);
        }

        [OnEventFire]
        public void UpdateRemoteWeapon(TimeUpdateEvent e, SingleNode<RemoteTankComponent> remoteTank,
            [JoinByTank] WeaponNode weapon,
            [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder) =>
            UpdateWeaponRotation(weapon, mouseControlStateHolder, e.DeltaTime, true);

        void UpdateWeaponRotation(WeaponNode weapon, SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder,
            float dt, bool rem = false) {
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;

            if (weaponRotationControl.Centering) {
                weaponRotationControl.EffectiveControl = !(weaponRotationControl.Rotation < 180f) ? 1 : -1;
            } else {
                weaponRotationControl.EffectiveControl = weaponRotationControl.Control;
            }

            WeaponGyroscopeRotationComponent weaponGyroscopeRotation = weapon.weaponGyroscopeRotation;

            if (mouseControlStateHolder.component.MouseControlEnable &&
                weaponRotationControl.BlockRotate &&
                !weaponRotationControl.ForceGyroscopeEnabled) {
                weaponRotationControl.MouseRotationCumulativeHorizontalAngle -=
                    weaponGyroscopeRotation.DeltaAngleOfHullRotation;

                weaponRotationControl.MouseRotationCumulativeHorizontalAngle =
                    MathUtil.ClampAngle(weaponRotationControl.MouseRotationCumulativeHorizontalAngle *
                                        ((float)Math.PI / 180f)) *
                    57.29578f;

                weaponGyroscopeRotation.DeltaAngleOfHullRotation = 0f;
            }

            float acceleration = weapon.weaponRotation.Acceleration;
            float num = acceleration * weaponGyroscopeRotation.WeaponTurnCoeff * dt;
            float max = weapon.weaponRotation.Speed * weaponGyroscopeRotation.WeaponTurnCoeff;
            float num2 = 0f;
            float num3 = 0f;
            bool flag = false;
            float num5;
            float num4;

            do {
                num4 = weaponRotationControl.MouseRotationCumulativeHorizontalAngle;
                num5 = weaponRotationControl.EffectiveControl;
                weaponRotationControl.Speed += num;
                weaponRotationControl.Speed = Mathf.Clamp(weaponRotationControl.Speed, 0f, max);
                num2 = weaponRotationControl.Speed * dt;

                if (mouseControlStateHolder.component.MouseControlEnable) {
                    if (weaponRotationControl.ForceGyroscopeEnabled) {
                        if (!weaponRotationControl.BlockRotate) {
                            if (weaponRotationControl.Centering) {
                                float num6 = MathUtil.ClampAngle180(
                                    weaponRotationControl.Rotation + weaponGyroscopeRotation.DeltaAngleOfHullRotation);

                                num3 = Mathf.Clamp(0f - num6, 0f - num2, num2);
                                num5 = num3 / num2;
                            } else {
                                num3 = Mathf.Clamp(num4, 0f - num2, num2);
                                num5 = num3 / num2;
                            }

                            num4 -= num3;
                        }

                        num3 -= weaponGyroscopeRotation.DeltaAngleOfHullRotation;
                    } else if (!weaponRotationControl.BlockRotate) {
                        if (weaponRotationControl.Centering) {
                            num3 = num5 * num2;
                            float num7 = MathUtil.ClampAngle180(weaponRotationControl.Rotation);

                            if (Mathf.Abs(num7) < Mathf.Abs(num3)) {
                                num3 = 0f - num7;
                                num5 = num3 / num2;
                            }
                        } else {
                            num3 = num4 - weaponGyroscopeRotation.DeltaAngleOfHullRotation;
                            num3 = Mathf.Clamp(num3, 0f - num2, num2);
                            num5 = num3 / num2;
                        }

                        num4 -= num3;
                        num4 -= weaponGyroscopeRotation.DeltaAngleOfHullRotation;
                    }

                    if (weaponRotationControl.BlockRotate) {
                        num5 = 0f;
                    }
                } else {
                    num3 = num5 * num2;

                    if (weaponRotationControl.ForceGyroscopeEnabled) {
                        num3 -= weaponGyroscopeRotation.DeltaAngleOfHullRotation;
                    }
                }

                if (flag) {
                    flag = false;
                } else if (Math.Sign(weaponRotationControl.PrevDeltaRotaion) != Math.Sign(num3)) {
                    weaponRotationControl.Speed = 0f;
                    flag = true;
                }
            } while (flag);

            weaponRotationControl.MouseRotationCumulativeHorizontalAngle = num4;
            weaponRotationControl.EffectiveControl = num5;
            weaponRotationControl.PrevDeltaRotaion = num3;
            float rotation = weaponRotationControl.Rotation;
            float num8;

            if (weaponRotationControl.Centering && (rotation < 0f - num3 || 360f - rotation < num3)) {
                weaponRotationControl.Centering = false;
                num8 = 0f;
            } else {
                num8 = ClampAngle(rotation + num3);
            }

            weapon.weaponInstance.WeaponInstance.transform.localRotation = Quaternion.AngleAxis(num8, Vector3.up);
            weaponRotationControl.Rotation = num8;

            if (weaponRotationControl.PrevEffectiveControl != weaponRotationControl.EffectiveControl) {
                weaponRotationControl.PrevEffectiveControl = weaponRotationControl.EffectiveControl;
                weaponRotationControl.EffectiveControl = Math.Sign(weaponRotationControl.EffectiveControl);
                ScheduleEvent(BaseWeaponRotationUpdateEvent<WeaponRotationControlChangedEvent>.Instance, weapon);
            }
        }

        float ClampAngle(float value) {
            while (value < 0f) {
                value += 360f;
            }

            while (value >= 360f) {
                value -= 360f;
            }

            return value;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;

            public TankMovableComponent tankMovable;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponGyroscopeRotationComponent weaponGyroscopeRotation;

            public WeaponInstanceComponent weaponInstance;

            public WeaponRotationComponent weaponRotation;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        [Not(typeof(ShaftComponent))]
        public class SimpleWeaponNode : WeaponNode { }

        public class ShaftWeaponNode : WeaponNode {
            public ShaftComponent shaft;
        }

        public class SelfBattleUser : Node {
            public MouseControlStateHolderComponent mouseControlStateHolder;

            public SelfBattleUserComponent selfBattleUser;
        }
    }
}