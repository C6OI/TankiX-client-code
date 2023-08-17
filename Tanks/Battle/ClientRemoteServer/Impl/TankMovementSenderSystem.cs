using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public class TankMovementSenderSystem : ECSSystem {
        const float SEND_MAX_TIME = 2f;

        const float MAX_DISTANCE = 5f;

        const float MAX_DISTANCE_Y = 2f;

        const float MAX_DISTANCE_SQUARED = 25f;

        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void AddComponent(NodeAddedEvent e, TankInitNode tank) =>
            tank.Entity.AddComponent<TankMovementSenderComponent>();

        [OnEventFire]
        public void OnUpdate(UpdateEvent e, SelfTankNode selfTankNode, [JoinByTank] WeaponNode weaponNode) {
            if (NeedRegularCorrection(selfTankNode) || NeedMandatoryCorrection(selfTankNode)) {
                CollectCommandsAndSend(selfTankNode, weaponNode, MoveCommandType.FULL);
            }
        }

        [OnEventFire]
        public void InitializeClientTime(NodeAddedEvent e, SelfTankNode selfTankNode, [JoinByTank] WeaponNode weaponNode) {
            ScheduleEvent<InitializeTimeCheckerEvent>(selfTankNode);
            CollectCommandsAndSend(selfTankNode, weaponNode, MoveCommandType.FULL);
        }

        [OnEventComplete]
        public void UpdateLastPosition(FixedUpdateEvent e, SelfTankNode tank) =>
            tank.tankMovementSender.LastPhysicsMovement = GetMovement(tank);

        [OnEventFire]
        public void OnAfterFixedUpdate(AfterFixedUpdateEvent e, SelfTankNode selfTankNode,
            [JoinByTank] WeaponNode weaponNode) {
            TankMovementSenderComponent tankMovementSender = selfTankNode.tankMovementSender;
            Rigidbody rigidbody = selfTankNode.rigidbody.Rigidbody;
            Vector3 velocity = rigidbody.velocity;
            float num = SqrMagnitudeXZ(velocity);
            Vector3 velocity2 = tankMovementSender.LastPhysicsMovement.Velocity;

            if (!tankMovementSender.LastSentMovement.HasValue ||
                SqrMagnitudeXZ(velocity2) > num + 0.01 &&
                SqrMagnitudeXZ(tankMovementSender.LastSentMovement.Value.Velocity) + 0.01 < SqrMagnitudeXZ(velocity2) ||
                IsXZDistanceTooLarge(tankMovementSender.LastSentMovement.Value.Position, rigidbody.position) ||
                IsYDistanceTooLarge(tankMovementSender.LastSentMovement.Value.Position, rigidbody.position)) {
                CollectCommandsAndSend(selfTankNode, weaponNode, MoveCommandType.TANK);
            }
        }

        bool IsYDistanceTooLarge(Vector3 lastPosition, Vector3 position) => Mathf.Abs(position.y - lastPosition.y) > 2f;

        bool IsXZDistanceTooLarge(Vector3 lastPosition, Vector3 position) {
            float num = position.x - lastPosition.x;
            float num2 = position.z - lastPosition.z;
            return num * num + num2 * num2 > 25f;
        }

        [OnEventFire]
        public void SendTankMovement(SendTankMovementEvent e, SelfTankNode selfTankNode,
            [JoinByTank] WeaponNode weaponNode) => CollectCommandsAndSend(selfTankNode, weaponNode, MoveCommandType.TANK);

        bool NeedRegularCorrection(SelfTankNode selfTankNode) {
            TankMovementSenderComponent tankMovementSender = selfTankNode.tankMovementSender;
            return PreciseTime.Time >= tankMovementSender.LastSentMovementTime + 2.0;
        }

        bool NeedMandatoryCorrection(SelfTankNode selfTankNode) {
            TankMovementSenderComponent tankMovementSender = selfTankNode.tankMovementSender;

            if (!tankMovementSender.LastSentMovement.HasValue) {
                return true;
            }

            if (selfTankNode.tankCollision.HasCollision != tankMovementSender.LastHasCollision) {
                tankMovementSender.LastHasCollision = selfTankNode.tankCollision.HasCollision;
                return true;
            }

            return false;
        }

        [OnEventFire]
        public void OnChassisControlChanged(ChassisControlChangedEvent e, SelfTankNode selfTankNode,
            [JoinByTank] WeaponNode weaponNode) => CollectCommandsAndSend(selfTankNode, weaponNode, MoveCommandType.TANK);

        [OnEventFire]
        public void OnWeaponRotationControlChanged(WeaponRotationControlChangedEvent e, WeaponNode weaponNode,
            [JoinByTank] SelfTankNode selfTankNode) =>
            CollectCommandsAndSend(selfTankNode, weaponNode, MoveCommandType.WEAPON);

        void CollectCommandsAndSend(SelfTankNode tankNode, WeaponNode weaponNode, MoveCommandType commandType) {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;

            if ((commandType & MoveCommandType.TANK) != 0 && !(tankMovementSender.LastSentMovementTime < PreciseTime.Time)) {
                commandType ^= MoveCommandType.TANK;
            }

            if ((commandType & MoveCommandType.WEAPON) != 0 &&
                !(tankMovementSender.LastSentWeaponRotationTime < PreciseTime.Time)) {
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
            moveCommand.DiscreteTankControl = GetControl(tankNode, weaponNode);
            moveCommand.ClientTime = PreciseTime.Time;
            MoveCommand moveCommand2 = moveCommand;
            SendCommand(tankNode, moveCommand2);
        }

        DiscreteTankControl GetControl(SelfTankNode tankNode, WeaponNode weaponNode) {
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

        Movement GetMovement(SelfTankNode tankNode) {
            if (PreciseTime.TimeType == TimeType.LAST_FIXED) {
                return tankNode.tankMovementSender.LastPhysicsMovement;
            }

            Rigidbody rigidbody = tankNode.rigidbody.Rigidbody;
            TankCollidersUnityComponent tankCollidersUnity = tankNode.tankCollidersUnity;
            Movement result = default;
            result.Position = TankPositionConverter.ConvertedSentToServer(tankCollidersUnity);
            result.Orientation = rigidbody.transform.rotation;
            result.Velocity = GetVelocity(tankNode);
            result.AngularVelocity = rigidbody.angularVelocity;
            return result;
        }

        Vector3 GetVelocity(SelfTankNode tankNode) {
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

        void SendCommand(SelfTankNode tankNode, MoveCommand moveCommand) {
            TankMovementSenderComponent tankMovementSender = tankNode.tankMovementSender;
            Movement? movement = moveCommand.Movement;

            if (movement.HasValue) {
                tankMovementSender.LastSentMovement = movement;
                tankMovementSender.LastSentMovementTime = PreciseTime.Time;
                Log.Debug("SEND MOVEMENT");
            }

            if (moveCommand.WeaponRotation.HasValue) {
                tankMovementSender.LastSentWeaponRotationTime = PreciseTime.Time;
                Log.Debug("SEND WEAPON_ROTATION");
            }

            ScheduleEvent(new MoveCommandEvent(moveCommand), tankNode.Entity);
            Log.Debug("SEND DISCRETE");
        }

        [OnEventFire]
        public void SendShotToServerEvent(BeforeShotEvent e, WeaponNode weaponNode, [JoinByTank] SelfTankNode selfTank) =>
            CollectCommandsAndSend(selfTank, weaponNode, MoveCommandType.FULL);

        float SqrMagnitudeXZ(Vector3 vector) => vector.x * vector.x + vector.z * vector.z;

        public class TankInitNode : Node {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;

            public TankMovableComponent tankMovable;
        }

        public class SelfTankNode : Node {
            public ChassisComponent chassis;

            public ChassisSmootherComponent chassisSmoother;

            public RigidbodyComponent rigidbody;

            public SelfTankComponent selfTank;

            public TankCollidersUnityComponent tankCollidersUnity;

            public TankCollisionComponent tankCollision;
            public TankGroupComponent tankGroup;

            public TankMovableComponent tankMovable;

            public TankMovementSenderComponent tankMovementSender;
        }

        public class SelfAnyStateTankNode : Node {
            public ChassisComponent chassis;

            public RigidbodyComponent rigidbody;

            public SelfTankComponent selfTank;

            public TankCollidersUnityComponent tankCollidersUnity;

            public TankCollisionComponent tankCollision;
            public TankGroupComponent tankGroup;

            public TankMovementSenderComponent tankMovementSender;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponRotationControlComponent weaponRotationControl;
        }
    }
}