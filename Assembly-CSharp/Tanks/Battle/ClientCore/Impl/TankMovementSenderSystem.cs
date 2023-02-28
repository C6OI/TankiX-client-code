using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientProtocol.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankMovementSenderSystem : ECSSystem {
        const float SEND_MAX_TIME = 2f;

        const float MAX_DISTANCE = 5f;

        const float MAX_DISTANCE_Y = 1.5f;

        const float MAX_ACCELERATION_Y = 20f;

        const float MAX_DISTANCE_SQUARED = 25f;

        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void AddComponent(NodeAddedEvent e, SingleNode<SelfTankComponent> selfTank) {
            selfTank.Entity.AddComponent<TankSyncComponent>();
        }

        [OnEventFire]
        public void AddComponent(NodeAddedEvent e, TankInitNode tank) {
            tank.Entity.AddComponent<TankMovementSenderComponent>();
        }

        [OnEventFire]
        public void RemoveComponent(NodeRemoveEvent e, TankInitNode tank) {
            tank.Entity.RemoveComponent<TankMovementSenderComponent>();
        }

        [OnEventFire]
        public void OnReset(NodeAddedEvent e, TankIncarnationNode incarnation, [JoinByTank] TankNode tankNode) {
            tankNode.tankMovementSender.LastSentMovementTime = 0.0;
        }

        [OnEventFire]
        public void OnUpdate(UpdateEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode) {
            if (NeedRegularCorrection(tankNode) || NeedMandatoryCorrection(tankNode)) {
                CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.FULL);
            }

            if (FloatCodec.DECODE_NAN_ERRORS != 0 || FloatCodec.ENCODE_NAN_ERRORS != 0) {
                long num = !tankNode.Entity.HasComponent<BattleGroupComponent>() ? 0 : tankNode.Entity.GetComponent<BattleGroupComponent>().Key;

                Log.ErrorFormat("NaN detected: battle={0} DECODE_NAN_ERRORS={1} ENCODE_NAN_ERRORS={2} encodeErrorStack={3}", num, FloatCodec.DECODE_NAN_ERRORS,
                    FloatCodec.ENCODE_NAN_ERRORS, FloatCodec.encodeErrorStack == null ? "null" : FloatCodec.encodeErrorStack.StackTrace);

                FloatCodec.DECODE_NAN_ERRORS = 0;
                FloatCodec.ENCODE_NAN_ERRORS = 0;
            }
        }

        [OnEventFire]
        public void InitializeClientTime(NodeAddedEvent e, TankInitNode tankNode, [JoinByTank] [Context] WeaponNode weaponNode) {
            ScheduleEvent<InitializeTimeCheckerEvent>(tankNode);
        }

        [OnEventFire]
        public void InitializeClientTime(NodeAddedEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode) {
            ScheduleEvent<InitializeTimeCheckerEvent>(tankNode);
            CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.FULL);
        }

        [OnEventComplete]
        public void UpdateLastPosition(FixedUpdateEvent e, TankNode tank) {
            tank.tankMovementSender.LastPhysicsMovement = GetMovement(tank);
        }

        [OnEventComplete]
        public void OnAfterFixedUpdate(AfterFixedUpdateEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode) {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;
            Rigidbody rigidbody = tankNode.rigidbody.Rigidbody;
            Vector3 velocity = rigidbody.velocity;
            float num = SqrMagnitudeXZ(velocity);
            Vector3 velocity2 = tankMovementSender.LastPhysicsMovement.Velocity;

            if (!tankMovementSender.LastSentMovement.HasValue ||
                (Mathf.Clamp(velocity.y, 0f, float.MaxValue) - Mathf.Clamp(velocity2.y, 0f, float.MaxValue)) / Time.fixedDeltaTime > 20f ||
                SqrMagnitudeXZ(velocity2) > num + 0.01 && SqrMagnitudeXZ(tankMovementSender.LastSentMovement.Value.Velocity) + 0.01 < SqrMagnitudeXZ(velocity2) ||
                IsXZDistanceTooLarge(tankMovementSender.LastSentMovement.Value.Position, rigidbody.position) ||
                IsYDistanceTooLarge(tankMovementSender.LastSentMovement.Value.Position, rigidbody.position)) {
                CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.TANK);
            }
        }

        bool IsYDistanceTooLarge(Vector3 lastPosition, Vector3 position) => Mathf.Abs(position.y - lastPosition.y) > 1.5f;

        bool IsXZDistanceTooLarge(Vector3 lastPosition, Vector3 position) {
            float num = position.x - lastPosition.x;
            float num2 = position.z - lastPosition.z;
            return num * num + num2 * num2 > 25f;
        }

        [OnEventFire]
        public void SendTankMovement(SendTankMovementEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode) {
            CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.TANK);
        }

        bool NeedRegularCorrection(TankNode tankNode) {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;
            return PreciseTime.Time >= tankMovementSender.LastSentMovementTime + 2.0;
        }

        bool NeedMandatoryCorrection(TankNode tankNode) {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;

            if (!tankMovementSender.LastSentMovement.HasValue) {
                return true;
            }

            if (tankNode.tankCollision.HasCollision != tankMovementSender.LastHasCollision) {
                tankMovementSender.LastHasCollision = tankNode.tankCollision.HasCollision;
                return true;
            }

            return false;
        }

        [OnEventFire]
        public void OnChassisControlChanged(ChassisControlChangedEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode) {
            CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.TANK);
        }

        [OnEventFire]
        public void OnEMPEffectTargetsCollection(SynchronizeSelfTankPositionBeforeEffectEvent e, TankNode tankNode, [JoinByTank] WeaponNode weaponNode) {
            CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.TANK);
        }

        [OnEventFire]
        public void OnWeaponRotationControlChanged(WeaponRotationControlChangedEvent e, WeaponNode weaponNode, [JoinByTank] TankNode tankNode) {
            CollectCommandsAndSend(tankNode, weaponNode, MoveCommandType.WEAPON);
        }

        void CollectCommandsAndSend(TankNode tankNode, WeaponNode weaponNode, MoveCommandType commandType) {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;

            if ((commandType & MoveCommandType.TANK) != 0 && !(tankMovementSender.LastSentMovementTime < PreciseTime.Time)) {
                commandType ^= MoveCommandType.TANK;
            }

            if ((commandType & MoveCommandType.WEAPON) != 0 && !(tankMovementSender.LastSentWeaponRotationTime < PreciseTime.Time)) {
                commandType ^= MoveCommandType.WEAPON;
            }

            Movement? movement = null;
            float? weaponRotation = null;

            if ((commandType & MoveCommandType.TANK) != 0) {
                movement = GetMovement(tankNode);
            }

            if ((commandType & MoveCommandType.WEAPON) != 0) {
                weaponRotation = weaponNode.weaponRotationControl.Rotation;
            }

            MoveCommand moveCommand = default;
            moveCommand.Movement = movement;
            moveCommand.WeaponRotation = weaponRotation;
            moveCommand.TankControlVertical = GetMoveAxis(tankNode);
            moveCommand.TankControlHorizontal = GetTurnAxis(tankNode);
            moveCommand.WeaponRotationControl = GetWeaponControl(weaponNode);
            moveCommand.ClientTime = (int)(PreciseTime.Time * 1000.0);
            MoveCommand moveCommand2 = moveCommand;
            SendCommand(tankNode, moveCommand2);
        }

        float GetMoveAxis(TankNode tankNode) => tankNode.chassis.EffectiveMoveAxis;

        float GetTurnAxis(TankNode tankNode) => tankNode.chassis.EffectiveTurnAxis;

        float GetWeaponControl(WeaponNode weaponNode) => weaponNode.weaponRotationControl.EffectiveControl;

        DiscreteTankControl GetControl(TankNode tankNode, WeaponNode weaponNode) {
            DiscreteTankControl result = default;
            ChassisComponent chassis = tankNode.chassis;
            MoveControl moveControl = default;
            moveControl.MoveAxis = chassis.EffectiveMoveAxis;
            moveControl.TurnAxis = chassis.EffectiveTurnAxis;
            result.TurnAxis = Mathf.RoundToInt(moveControl.TurnAxis);
            result.MoveAxis = Mathf.RoundToInt(moveControl.MoveAxis);
            result.WeaponControl = Mathf.RoundToInt(weaponNode.weaponRotationControl.EffectiveControl);
            return result;
        }

        Movement GetMovement(TankNode tankNode) {
            if (PreciseTime.TimeType == TimeType.LAST_FIXED) {
                return tankNode.tankMovementSender.LastPhysicsMovement;
            }

            Rigidbody rigidbody = tankNode.rigidbody.Rigidbody;
            Transform rigidbodyTransform = tankNode.rigidbody.RigidbodyTransform;
            TankCollidersUnityComponent tankCollidersUnity = tankNode.tankCollidersUnity;
            Movement result = default;
            result.Position = TankPositionConverter.ConvertedSentToServer(tankCollidersUnity);
            result.Orientation = rigidbodyTransform.rotation;
            result.Velocity = GetVelocity(tankNode);
            result.AngularVelocity = rigidbody.angularVelocity;
            return result;
        }

        Vector3 GetVelocity(TankNode tankNode) {
            Vector3 velocity = tankNode.rigidbody.Rigidbody.velocity;
            float num = SqrMagnitudeXZ(velocity);
            float currentValue = tankNode.chassisSmoother.maxSpeedSmoother.CurrentValue;

            if (num > currentValue * currentValue) {
                Vector3 vector = velocity;
                vector.y = 0f;
                velocity += (currentValue - Mathf.Sqrt(num)) * vector.normalized;
            }

            return velocity;
        }

        void SendCommand(TankNode tankNode, MoveCommand moveCommand) {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;
            Movement? movement = moveCommand.Movement;

            if (movement.HasValue) {
                if (!PhysicsUtil.ValidateMovement(movement.Value)) {
                    return;
                }

                tankMovementSender.LastSentMovement = movement;
                tankMovementSender.LastSentMovementTime = PreciseTime.Time;
                Log.Debug("SEND MOVEMENT");
            }

            if (moveCommand.WeaponRotation.HasValue) {
                if (!PhysicsUtil.IsValidFloat(moveCommand.WeaponRotation.Value)) {
                    LoggerProvider.GetLogger(typeof(PhysicsUtil)).ErrorFormat("Invalid WeaponRotation. StackTrace:[{0}]", Environment.StackTrace);
                    return;
                }

                tankMovementSender.LastSentWeaponRotationTime = PreciseTime.Time;
                Log.Debug("SEND WEAPON_ROTATION");
            }

            ScheduleEvent(new MoveCommandEvent(moveCommand), tankNode.Entity);
            Log.Debug("SEND DISCRETE");
        }

        [OnEventFire]
        public void SendShotToServerEvent(BeforeShotEvent e, WeaponNode weaponNode, [JoinByTank] TankNode tank) {
            CollectCommandsAndSend(tank, weaponNode, MoveCommandType.FULL);
        }

        float SqrMagnitudeXZ(Vector3 vector) => vector.x * vector.x + vector.z * vector.z;

        public class TankInitNode : Node {
            public TankGroupComponent tankGroup;

            public TankMovableComponent tankMovable;

            public TankSyncComponent tankSync;
        }

        public class TankNode : Node {
            public ChassisComponent chassis;

            public ChassisSmootherComponent chassisSmoother;

            public RigidbodyComponent rigidbody;

            public TankCollidersUnityComponent tankCollidersUnity;

            public TankCollisionComponent tankCollision;
            public TankGroupComponent tankGroup;

            public TankMovableComponent tankMovable;

            public TankMovementSenderComponent tankMovementSender;

            public TankSyncComponent tankSync;
        }

        public class AnyStateTankNode : Node {
            public ChassisComponent chassis;

            public RigidbodyComponent rigidbody;

            public TankCollidersUnityComponent tankCollidersUnity;

            public TankCollisionComponent tankCollision;
            public TankGroupComponent tankGroup;

            public TankMovementSenderComponent tankMovementSender;

            public TankSyncComponent tankSync;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }
    }
}