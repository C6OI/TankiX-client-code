using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankEngineSystem : ECSSystem {
        [OnEventFire]
        public void InitTankEngine(NodeAddedEvent evt, SingleNode<TankEngineConfigComponent> tank) {
            TankEngineConfigComponent component = tank.component;
            TankEngineComponent tankEngineComponent = new();
            tankEngineComponent.MovingBorder = component.MinEngineMovingBorder;
            tankEngineComponent.Value = 0f;
            tankEngineComponent.CollisionTimerSec = 0f;
            tankEngineComponent.HasValuableCollision = false;
            tank.Entity.AddComponent(tankEngineComponent);
        }

        [OnEventFire]
        public void ResetTankEngine(NodeAddedEvent evt, TankEngineMovableNode engine) {
            engine.chassis.Reset();
        }

        [OnEventFire]
        public void RecalculateParametersOnSpeedEffectStart(NodeAddedEvent evt, SpeedEffectNode effect, [Context] [JoinByTank] TankEngineNode tank) {
            TankEngineComponent tankEngine = tank.tankEngine;
            TankEngineConfigComponent tankEngineConfig = tank.tankEngineConfig;
            tankEngine.MovingBorder = tankEngineConfig.MaxEngineMovingBorder;
        }

        [OnEventFire]
        public void RecalculateParametersOnSpeedEffectStop(NodeRemoveEvent evt, SpeedEffectNode effect, [JoinByTank] TankEngineNode tank) {
            TankEngineComponent tankEngine = tank.tankEngine;
            TankEngineConfigComponent tankEngineConfig = tank.tankEngineConfig;
            tankEngine.MovingBorder = tankEngineConfig.MinEngineMovingBorder;
        }

        [OnEventFire]
        public void UpdateTankEngine(FixedUpdateEvent evt, TankEngineMovableNode tank) {
            TankEngineComponent tankEngine = tank.tankEngine;
            TankEngineConfigComponent tankEngineConfig = tank.tankEngineConfig;
            TrackComponent track = tank.track;
            TankCollisionComponent tankCollision = tank.tankCollision;
            ChassisComponent chassis = tank.chassis;
            float effectiveMoveAxis = chassis.EffectiveMoveAxis;
            float effectiveTurnAxis = chassis.EffectiveTurnAxis;
            bool hasCollision = tankCollision.HasCollision;

            if (hasCollision != tankEngine.HasValuableCollision) {
                tankEngine.CollisionTimerSec += evt.DeltaTime;
            } else {
                tankEngine.CollisionTimerSec = 0f;
            }

            if (tankEngine.CollisionTimerSec >= tankEngineConfig.EngineCollisionIntervalSec) {
                tankEngine.HasValuableCollision = hasCollision;
            }

            if (effectiveMoveAxis == 0f) {
                if (effectiveTurnAxis == 0f) {
                    tankEngine.Value = 0f;
                } else {
                    UpdateTankEngine(tankEngine, tankEngineConfig, tankEngine.HasValuableCollision, track, evt.DeltaTime, effectiveTurnAxis, tank.speedConfig.TurnAcceleration,
                        tank.speedConfig.ReverseTurnAcceleration, tank.speed.TurnSpeed, tank.tankEngineConfig.EngineTurningBorder);
                }
            } else {
                UpdateTankEngine(tankEngine, tankEngineConfig, tankEngine.HasValuableCollision, track, evt.DeltaTime, effectiveMoveAxis, tank.speed.Acceleration,
                    tank.speedConfig.ReverseAcceleration, tank.speed.Speed, tankEngine.MovingBorder);
            }
        }

        void UpdateTankEngine(TankEngineComponent tankEngine, TankEngineConfigComponent tankEngineConfig, bool hasCollision, TrackComponent track, float dt, float currentAxis,
            float straightAcceleration, float reverseAcceleration, float maxSpeed, float border) {
            int num = track.LeftTrack.numContacts + track.RightTrack.numContacts;
            float num2 = border;

            if (num > 0 && hasCollision) {
                num2 = tankEngineConfig.MaxEngineMovingBorder;
            }

            float num3 = !(currentAxis > 0f) ? reverseAcceleration : straightAcceleration;
            float num4 = num2 * num3 / maxSpeed;
            tankEngine.Value += num4 * dt;
            tankEngine.Value = Mathf.Min(tankEngine.Value, num2);
        }

        public class TankEngineNode : Node {
            public SpeedComponent speed;

            public SpeedConfigComponent speedConfig;

            public TankEngineComponent tankEngine;
            public TankEngineConfigComponent tankEngineConfig;

            public TankGroupComponent tankGroup;
        }

        public class TankEngineMovableNode : Node {
            public ChassisComponent chassis;

            public SpeedComponent speed;

            public SpeedConfigComponent speedConfig;

            public TankCollisionComponent tankCollision;

            public TankEngineComponent tankEngine;
            public TankEngineConfigComponent tankEngineConfig;

            public TankGroupComponent tankGroup;

            public TankMovableComponent tankMovable;

            public TrackComponent track;
        }

        public class SpeedEffectNode : Node {
            public TankGroupComponent tankGroup;
            public TurboSpeedEffectComponent turboSpeedEffect;
        }
    }
}