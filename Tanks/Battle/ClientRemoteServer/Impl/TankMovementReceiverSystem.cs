using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public class TankMovementReceiverSystem : ECSSystem {
        static readonly float PATH_SMALL_DISTANCE = 5f;

        static readonly float BIG_ROTATION_DEGREES = 60f;

        static readonly Vector3 PATH_OFFSET = Vector3.up * 0.2f;

        [OnEventFire]
        public void OnTankAdded(NodeAddedEvent e, TankNode tank, [JoinByTank] [Context] WeaponNode weapon) {
            Log.DebugFormat("INIT {0}", tank);
            TankMovementComponent tankMovement = tank.tankMovement;
            Movement? movement = tankMovement.Movement;
            ApplyMovement(tank, ref movement, true);

            ApplyMoveControl(tank,
                new MoveControl {
                    MoveAxis = tankMovement.MoveControl.MoveAxis,
                    TurnAxis = tankMovement.MoveControl.TurnAxis
                });

            ApplyWeaponRotation(weapon, tankMovement.WeaponRotation);
            ApplyWeaponControl(weapon, tankMovement.WeaponControl);
            ScheduleEvent<TankMovementInitEvent>(tank);
        }

        [OnEventFire]
        public void OnMoveCommandDiscrete(MoveCommandServerEvent e, TankNode tank, [JoinByTank] WeaponNode weapon) {
            Log.Debug("RECEIVE DISCRETE");
            DiscreteTankControl discreteTankControl = e.MoveCommand.DiscreteTankControl;
            Movement? movement = e.MoveCommand.Movement;
            ApplyMovement(tank, ref movement, false);

            ApplyMoveControl(tank,
                new MoveControl {
                    MoveAxis = discreteTankControl.MoveAxis,
                    TurnAxis = discreteTankControl.TurnAxis
                });

            ApplyWeaponRotation(weapon, e.MoveCommand.WeaponRotation);
            ApplyWeaponControl(weapon, discreteTankControl.WeaponControl);
        }

        [OnEventFire]
        public void OnMoveCommandAnalog(AnalogMoveCommandServerEvent e, TankNode tank, [JoinByTank] WeaponNode weapon) {
            Log.Debug("RECEIVE ANALOG");
            Movement? movement = e.Movement;
            ApplyMovement(tank, ref movement, false);
            ApplyMoveControl(tank, e.MoveControl);
            ApplyWeaponRotation(weapon, e.WeaponRotation);
            ApplyWeaponControl(weapon, e.WeaponControl);
        }

        void ApplyMovement(TankNode tank, ref Movement? movement, bool init) {
            if (movement.HasValue) {
                Movement movement2 = movement.Value;
                bool flag = HalveMoveCommandIfNeed(tank, init, ref movement2);
                Log.Debug(!flag ? "APPLY MOVEMENT" : "APPLY HALVED MOVEMENT");
                Transform transform = tank.rigidbody.Rigidbody.transform;
                Rigidbody rigidbody = tank.rigidbody.Rigidbody;
                transform.rotation = movement2.Orientation;

                transform.position = TankPositionConverter.ConvertedReceptionFromServer(movement2.Position,
                    tank.tankCollidersUnity,
                    transform.position);

                rigidbody.velocity = movement2.Velocity;
                rigidbody.angularVelocity = movement2.AngularVelocity;
            }
        }

        static bool IsTankDead(TankNode tank) => tank.Entity.HasComponent<TankDeadStateComponent>();

        bool HalveMoveCommandIfNeed(TankNode tank, bool init, ref Movement movement) {
            if (init || tank.Entity.HasComponent<TankDeadStateComponent>()) {
                return false;
            }

            Transform transform = tank.rigidbody.Rigidbody.transform;
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            Movement currentMoveDump = DumpMovement(transform, rigidbody, tank.tankCollidersUnity);

            if (!IsMovePathClean(ref currentMoveDump, ref movement)) {
                return false;
            }

            if (IsBigRotation(ref movement, ref currentMoveDump)) {
                return false;
            }

            movement = InterpolateMoveCommand(ref currentMoveDump, ref movement, 0.5f);
            return true;
        }

        bool IsMovePathClean(ref Movement currentMoveDump, ref Movement movement) {
            Vector3 vector = movement.Position - currentMoveDump.Position;
            float num = Vector3.SqrMagnitude(vector);

            if (num < PATH_SMALL_DISTANCE * PATH_SMALL_DISTANCE) {
                return true;
            }

            return !Physics.Raycast(currentMoveDump.Position + PATH_OFFSET, vector, Mathf.Sqrt(num), LayerMasks.STATIC);
        }

        bool IsBigRotation(ref Movement movement, ref Movement currentMovement) =>
            Quaternion.Angle(currentMovement.Orientation, movement.Orientation) > BIG_ROTATION_DEGREES;

        Movement DumpMovement(Transform transform, Rigidbody rigidbody, TankCollidersUnityComponent tankCollidersUnity) {
            Movement result = default;
            result.Position = TankPositionConverter.ConvertedSentToServer(tankCollidersUnity);
            result.Orientation = transform.rotation;
            result.Velocity = rigidbody.velocity;
            result.AngularVelocity = rigidbody.angularVelocity;
            return result;
        }

        Movement InterpolateMoveCommand(ref Movement moveCommand1, ref Movement moveCommand2, float interpolationCoeff) {
            Movement result = default;
            result.Position = Vector3.Lerp(moveCommand1.Position, moveCommand2.Position, interpolationCoeff);
            result.Orientation = Quaternion.Slerp(moveCommand1.Orientation, moveCommand2.Orientation, interpolationCoeff);
            result.Velocity = Vector3.Lerp(moveCommand1.Velocity, moveCommand2.Velocity, interpolationCoeff);

            result.AngularVelocity =
                Vector3.Slerp(moveCommand1.AngularVelocity, moveCommand2.AngularVelocity, interpolationCoeff);

            return result;
        }

        void ApplyMoveControl(TankNode tank, MoveControl? moveControl) {
            if (moveControl.HasValue) {
                ApplyMoveControl(tank.chassis, moveControl.Value.MoveAxis, moveControl.Value.TurnAxis);
            }
        }

        void ApplyMoveControl(ChassisComponent chassis, float moveAxis, float turnAxis) {
            Log.Debug("APPLY MOVE_CONTROL");
            chassis.MoveAxis = moveAxis;
            chassis.TurnAxis = turnAxis;
        }

        void ApplyWeaponRotation(WeaponNode weapon, float? weaponRotation) {
            if (weaponRotation.HasValue) {
                ApplyWeaponRotation(weapon.weaponInstance, weaponRotation.Value);
                weapon.weaponRotationControl.Rotation = weaponRotation.Value;
            }
        }

        void ApplyWeaponRotation(WeaponInstanceComponent weaponInstanceComponent, float weaponRotation) {
            Log.Debug("APPLY WEAPON_ROTATION");
            Transform transform = weaponInstanceComponent.WeaponInstance.transform;
            transform.localRotation = Quaternion.AngleAxis(weaponRotation, Vector3.up);
        }

        void ApplyWeaponControl(WeaponNode weapon, float? weaponControl) =>
            ApplyWeaponControl(weapon.weaponRotationControl, weaponControl);

        void ApplyWeaponControl(WeaponRotationControlComponent weaponRotationComponent, float? weaponControl) {
            if (weaponControl.HasValue) {
                Log.Debug("APPLY WEAPON_CONTROL");
                weaponRotationComponent.Control = weaponControl.Value;
            }
        }

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;

            public ChassisComponent chassis;

            public RigidbodyComponent rigidbody;

            public TankComponent tank;

            public TankCollidersUnityComponent tankCollidersUnity;
            public TankGroupComponent tankGroup;

            public TankMovementComponent tankMovement;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;

            public WeaponRotationControlComponent weaponRotationControl;
        }
    }
}