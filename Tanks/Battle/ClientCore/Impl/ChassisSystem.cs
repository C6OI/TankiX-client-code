using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ChassisSystem : ECSSystem {
        const int SELF_TANK_UPDATE_TRACK_FPS = 30;

        const int REMOTE_TANK_UPDATE_TRACK_FPS = 15;

        const float MIN_ACCELERATION = 4f;

        const float SQRT1_2 = 0.70710677f;

        const float FULL_FORCE_ANGLE = (float)Math.PI / 4f;

        const float ZERO_FORCE_ANGLE = (float)Math.PI / 3f;

        const float FULL_SLOPE_ANGLE = (float)Math.PI / 4f;

        const float MAX_SLOPE_ANGLE = (float)Math.PI / 3f;

        static readonly string RIGHT_AXIS = "MoveRight";

        static readonly string LEFT_AXIS = "MoveLeft";

        static readonly string FORWARD_AXIS = "MoveForward";

        static readonly string BACKWARD_AXIS = "MoveBackward";

        static readonly float FULL_SLOPE_COS_ANGLE = Mathf.Cos((float)Math.PI / 4f);

        static readonly float MAX_SLOPE_COS_ANGLE = Mathf.Cos((float)Math.PI / 3f);

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void InitTankChassis(NodeAddedEvent evt, ChassisInitNode node) {
            ChassisComponent chassisComponent = new();
            CreateTracks(node, chassisComponent);
            node.Entity.AddComponent(chassisComponent);
            node.Entity.AddComponent<EffectiveSpeedComponent>();
            ChassisSmootherComponent chassisSmootherComponent = new();
            chassisSmootherComponent.maxSpeedSmoother.Reset(node.speed.Speed);
            chassisSmootherComponent.maxTurnSpeedSmoother.Reset(node.speed.TurnSpeed);
            node.Entity.AddComponent(chassisSmootherComponent);
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            rigidbody.mass = node.weight.Weight;
        }

        [OnEventFire]
        public void FixedUpdate(FixedUpdateEvent evt, ChassisNode chassisNode,
            [JoinAll] SingleNode<GameTankSettingsComponent> gameTankSettings,
            [JoinAll] Optional<SingleNode<BattleActionsStateComponent>> inputState) {
            bool inputEnabled = inputState.IsPresent();

            if (chassisNode.Entity.HasComponent<SelfTankComponent>()) {
                UpdateSelfInput(chassisNode, inputEnabled, gameTankSettings.component.MovementControlsInverted);
            }

            UpdateInput(chassisNode, inputEnabled);
            ChassisSmootherComponent chassisSmoother = chassisNode.chassisSmoother;
            chassisSmoother.maxSpeedSmoother.SetTargetValue(chassisNode.speed.Speed);
            float num = chassisSmoother.maxSpeedSmoother.Update(evt.DeltaTime);
            chassisNode.effectiveSpeed.MaxSpeed = num;
            Rigidbody rigidbody = chassisNode.rigidbody.Rigidbody;

            if ((bool)rigidbody) {
                float x = rigidbody.velocity.x;
                float z = rigidbody.velocity.z;

                if (x * x + z * z > num * num) {
                    float num2 = num / (float)Math.Sqrt(x * x + z * z);
                    Vector3 velocity = new(rigidbody.velocity.x * num2, rigidbody.velocity.y, rigidbody.velocity.z * num2);
                    rigidbody.velocity = velocity;
                }

                chassisSmoother.maxTurnSpeedSmoother.SetTargetValue(chassisNode.speed.TurnSpeed);
                chassisNode.effectiveSpeed.MaxTurnSpeed = chassisSmoother.maxTurnSpeedSmoother.Update(evt.DeltaTime);
                AdjustSuspensionSpringCoeff(chassisNode.chassisConfig, chassisNode.chassis, chassisNode.rigidbody.Rigidbody);
                UpdateSuspensionContacts(chassisNode.track, evt.DeltaTime);
                ApplyMovementForces(chassisNode, evt.DeltaTime);
                ApplyStaticFriction(chassisNode.track, chassisNode.rigidbody.Rigidbody);
            }
        }

        void UpdateInput(ChassisNode tank, bool inputEnabled) {
            ChassisComponent chassis = tank.chassis;
            bool flag = tank.Entity.HasComponent<TankMovableComponent>();
            chassis.EffectiveMoveAxis = !flag ? 0f : chassis.MoveAxis;
            chassis.EffectiveTurnAxis = !flag ? 0f : chassis.TurnAxis;
        }

        void UpdateSelfInput(ChassisNode tank, bool inputEnabled, bool inverse) {
            ChassisComponent chassis = tank.chassis;
            float num = !inputEnabled ? 0f : InputManager.GetUnityAxis(RIGHT_AXIS) - InputManager.GetUnityAxis(LEFT_AXIS);

            float num2 = !inputEnabled ? 0f
                             : InputManager.GetUnityAxis(FORWARD_AXIS) - InputManager.GetUnityAxis(BACKWARD_AXIS);

            if (inverse && num2 < 0f) {
                num *= -1f;
            }

            Vector2 vector = new(chassis.TurnAxis, chassis.MoveAxis);
            Vector2 vector2 = new(num, num2);

            if (vector2 != vector) {
                chassis.TurnAxis = num;
                chassis.MoveAxis = num2;
                bool flag = tank.Entity.HasComponent<TankMovableComponent>();
                chassis.EffectiveMoveAxis = !flag ? 0f : chassis.MoveAxis;
                chassis.EffectiveTurnAxis = !flag ? 0f : chassis.TurnAxis;
                ScheduleEvent<ChassisControlChangedEvent>(tank);
            }
        }

        public void ApplyStaticFriction(TrackComponent tracks, Rigidbody rigidbody) {
            if (tracks.RightTrack.numContacts >= tracks.RightTrack.rays.Length >> 1 ||
                tracks.LeftTrack.numContacts >= tracks.LeftTrack.rays.Length >> 1) {
                Vector3 up = rigidbody.transform.up;
                float num = Vector3.Dot(Physics.gravity, up);
                float num2 = 0.70710677f * Physics.gravity.magnitude;

                if (num < 0f - num2 || num > num2) {
                    Vector3 force = (up * num - Physics.gravity) * rigidbody.mass;
                    rigidbody.AddForce(force);
                }
            }
        }

        void CreateTracks(ChassisInitNode node, ChassisComponent chassis) {
            Entity entity = node.Entity;
            TankCollidersComponent tankColliders = node.tankColliders;
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            BoxCollider boundsCollider = tankColliders.BoundsCollider;
            float trackLength = boundsCollider.size.z * 0.8f;
            float num = boundsCollider.size.x - chassisConfig.TrackSeparation;
            Vector3 vector = boundsCollider.center - new Vector3(0f, boundsCollider.size.y / 2f, 0f);
            Vector3 trackCenterPosition = vector + new Vector3(-0.5f * num, chassisConfig.NominalRayLength, 0f);
            Vector3 trackCenterPosition2 = vector + new Vector3(0.5f * num, chassisConfig.NominalRayLength, 0f);
            float damping = node.damping.Damping;
            TrackComponent trackComponent = new();

            trackComponent.LeftTrack = new Track(rigidbody,
                chassisConfig.NumRaysPerTrack,
                trackCenterPosition,
                trackLength,
                chassisConfig,
                chassis,
                -1,
                damping);

            trackComponent.RightTrack = new Track(rigidbody,
                chassisConfig.NumRaysPerTrack,
                trackCenterPosition2,
                trackLength,
                chassisConfig,
                chassis,
                1,
                damping);

            int vISIBLE_FOR_CHASSIS_SEMI_ACTIVE = LayerMasks.VISIBLE_FOR_CHASSIS_SEMI_ACTIVE;
            trackComponent.LeftTrack.SetRayсastLayerMask(vISIBLE_FOR_CHASSIS_SEMI_ACTIVE);
            trackComponent.RightTrack.SetRayсastLayerMask(vISIBLE_FOR_CHASSIS_SEMI_ACTIVE);
            entity.AddComponent(trackComponent);
        }

        [OnEventFire]
        public void SetTankCollisionLayerMask(NodeAddedEvent e, TankActiveStateNode node) {
            int vISIBLE_FOR_CHASSIS_ACTIVE = LayerMasks.VISIBLE_FOR_CHASSIS_ACTIVE;
            node.track.LeftTrack.SetRayсastLayerMask(vISIBLE_FOR_CHASSIS_ACTIVE);
            node.track.RightTrack.SetRayсastLayerMask(vISIBLE_FOR_CHASSIS_ACTIVE);
        }

        void AdjustSuspensionSpringCoeff(ChassisConfigComponent chassisConfig, ChassisComponent chassis,
            Rigidbody rigidbody) {
            float num = Physics.gravity.magnitude * rigidbody.mass;

            chassis.SpringCoeff =
                num / (2 * chassisConfig.NumRaysPerTrack * (chassisConfig.MaxRayLength - chassisConfig.NominalRayLength));
        }

        void UpdateSuspensionContacts(TrackComponent trackComponent, float dt) {
            trackComponent.LeftTrack.UpdateSuspensionContacts(dt);
            trackComponent.RightTrack.UpdateSuspensionContacts(dt);
        }

        void ApplyMovementForces(ChassisNode node, float dt) {
            TrackComponent track = node.track;

            if (track.LeftTrack.numContacts + track.RightTrack.numContacts > 0) {
                Rigidbody rigidbody = node.rigidbody.Rigidbody;
                ChassisConfigComponent chassisConfig = node.chassisConfig;
                Vector3 surfaceVelocity;
                Vector3 angularSurfaceVelocity;
                CalculateNetSurfaceVelocities(node, out surfaceVelocity, out angularSurfaceVelocity);
                float num = CalculateSlopeCoefficient(rigidbody.transform.up.y);
                float forwardRelativeSpeed;

                rigidbody.velocity = CalculateRigidBodyVelocity(rigidbody,
                    surfaceVelocity,
                    node.speedConfig.SideAcceleration * num * dt,
                    out forwardRelativeSpeed);

                if (track.LeftTrack.numContacts > 0 || track.RightTrack.numContacts > 0) {
                    Vector3 vector =
                        CalculateRelativeAngularVelocity(node, dt, num, angularSurfaceVelocity, forwardRelativeSpeed);

                    rigidbody.angularVelocity = angularSurfaceVelocity + vector;
                }
            }
        }

        float CalculateSlopeCoefficient(float upAxisY) {
            float result = 1f;

            if (upAxisY < FULL_SLOPE_COS_ANGLE) {
                result = !(upAxisY < MAX_SLOPE_COS_ANGLE)
                             ? ((float)Math.PI / 3f - Mathf.Acos(upAxisY)) / ((float)Math.PI / 12f) : 0f;
            }

            return result;
        }

        Vector3 GetContactsMidpoint(ChassisConfigComponent chassisConfig, TrackComponent tracks) {
            Vector3 vector = default;

            for (int i = 0; i < chassisConfig.NumRaysPerTrack; i++) {
                SuspensionRay suspensionRay = tracks.LeftTrack.rays[i];

                if (suspensionRay.hasCollision) {
                    vector += suspensionRay.rayHit.point;
                }

                suspensionRay = tracks.RightTrack.rays[i];

                if (suspensionRay.hasCollision) {
                    vector += suspensionRay.rayHit.point;
                }
            }

            float num = tracks.LeftTrack.numContacts + tracks.RightTrack.numContacts;
            return vector / num;
        }

        void CalculateNetSurfaceVelocities(ChassisNode node, out Vector3 surfaceVelocity,
            out Vector3 angularSurfaceVelocity) {
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            TrackComponent track = node.track;
            Vector3 contactsMidpoint = GetContactsMidpoint(chassisConfig, track);
            surfaceVelocity = Vector3.zero;
            angularSurfaceVelocity = Vector3.zero;

            for (int i = 0; i < chassisConfig.NumRaysPerTrack; i++) {
                AddSurfaceVelocitiesFromRay(node,
                    track.LeftTrack.rays[i],
                    contactsMidpoint,
                    ref surfaceVelocity,
                    ref angularSurfaceVelocity);

                AddSurfaceVelocitiesFromRay(node,
                    track.RightTrack.rays[i],
                    contactsMidpoint,
                    ref surfaceVelocity,
                    ref angularSurfaceVelocity);
            }

            float num = track.LeftTrack.numContacts + track.RightTrack.numContacts;
            surfaceVelocity /= num;
            angularSurfaceVelocity /= num;
        }

        void AddSurfaceVelocitiesFromRay(ChassisNode node, SuspensionRay ray, Vector3 contactsMidpoint,
            ref Vector3 surfaceVelocity, ref Vector3 angularSurfaceVelocity) {
            if (ray.hasCollision) {
                surfaceVelocity += ray.surfaceVelocity;
                Vector3 lhs = ray.rayHit.point - contactsMidpoint;
                float sqrMagnitude = lhs.sqrMagnitude;
                float num = 0.0001f;

                if (sqrMagnitude > num) {
                    angularSurfaceVelocity += Vector3.Cross(lhs, ray.surfaceVelocity) / sqrMagnitude;
                }
            }
        }

        Vector3 CalculateRigidBodyVelocity(Rigidbody rigidbody, Vector3 surfaceVelocity, float sideSpeedDelta,
            out float forwardRelativeSpeed) {
            Vector3 right = rigidbody.transform.right;
            Vector3 forward = rigidbody.transform.forward;
            Vector3 vector = rigidbody.velocity - surfaceVelocity;
            forwardRelativeSpeed = Vector3.Dot(vector, forward);
            float num = CalculateSideVelocityDelta(vector, right, sideSpeedDelta);
            vector += num * right;
            return surfaceVelocity + vector;
        }

        float CalculateSideVelocityDelta(Vector3 relativeVelocity, Vector3 xAxis, float sideSpeedDelta) {
            float num = Vector3.Dot(relativeVelocity, xAxis);
            float num2 = num;

            if (num2 < 0f) {
                num2 = !(sideSpeedDelta > 0f - num2) ? num2 + sideSpeedDelta : 0f;
            } else if (num2 > 0f) {
                num2 = !(sideSpeedDelta > num2) ? num2 - sideSpeedDelta : 0f;
            }

            return num2 - num;
        }

        Vector3 CalculateRelativeAngularVelocity(ChassisNode node, float dt, float slopeCoeff,
            Vector3 surfaceAngularVelocity, float forwardRelativeSpeed) {
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            TrackComponent track = node.track;
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            float maxTurnSpeed = node.effectiveSpeed.MaxTurnSpeed * ((float)Math.PI / 180f);
            Vector3 up = rigidbody.transform.up;
            Vector3 forward = rigidbody.transform.forward;
            Vector3 vector = rigidbody.angularVelocity - surfaceAngularVelocity;
            float relativeTurnSpeed = Vector3.Dot(vector, up);
            float forcePerRay = CalculateForcePerRay(node, dt, forwardRelativeSpeed);

            for (int i = 0; i < chassisConfig.NumRaysPerTrack; i++) {
                ApplyForceFromRay(track.LeftTrack.rays[i], rigidbody, forward, forcePerRay);
                ApplyForceFromRay(track.RightTrack.rays[i], rigidbody, forward, forcePerRay);
            }

            float num = Vector3.Dot(vector, up);
            float num2 = RecalculateRelativeTurnSpeed(node, dt, maxTurnSpeed, relativeTurnSpeed, slopeCoeff) - num;
            return vector + num2 * up;
        }

        float CalculateForcePerRay(ChassisNode node, float dt, float forwardRelativeSpeed) {
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            ChassisComponent chassis = node.chassis;
            float maxSpeed = node.effectiveSpeed.MaxSpeed;
            TrackComponent track = node.track;
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            float num = node.speed.Acceleration;
            float num2 = 0f;

            if (chassis.EffectiveMoveAxis == 0f) {
                num2 = (0f - MathUtil.Sign(forwardRelativeSpeed)) * num * dt;

                if (MathUtil.Sign(forwardRelativeSpeed) != MathUtil.Sign(forwardRelativeSpeed + num2)) {
                    num2 = 0f - forwardRelativeSpeed;
                }
            } else {
                if (IsReversedMove(chassis.EffectiveMoveAxis, forwardRelativeSpeed)) {
                    num = node.speedConfig.ReverseAcceleration;
                }

                num2 = chassis.EffectiveMoveAxis * num * dt;
            }

            float num3 = Mathf.Clamp(forwardRelativeSpeed + num2, 0f - maxSpeed, maxSpeed);
            float num4 = num3 - forwardRelativeSpeed;
            float num5 = 1f;
            float num6 = Mathf.Clamp01(1f - Mathf.Abs(forwardRelativeSpeed / maxSpeed));

            if (num6 < num5 && chassis.EffectiveMoveAxis * MathUtil.Sign(forwardRelativeSpeed) > 0f) {
                num4 *= num6 / num5;
            }

            float num7 = num4 / dt;

            if (Mathf.Abs(num7) < 4f && Mathf.Abs(num3) > 0.5f * maxSpeed) {
                num7 = MathUtil.SignEpsilon(num7, 0.1f) * 4f;
            }

            float num8 = num7 * rigidbody.mass;
            int num9 = track.LeftTrack.numContacts + track.RightTrack.numContacts;
            int num10 = 2 * chassisConfig.NumRaysPerTrack;
            float num11 = num8 * (num9 + 0.42f * (num10 - track.LeftTrack.numContacts)) / num10;
            return num11 / num9;
        }

        bool IsReversedTurn(float turnDirection, float relativeTurnSpeed) => turnDirection * relativeTurnSpeed < 0f;

        bool IsReversedMove(float moveDirection, float relativeMovementSpeed) => moveDirection * relativeMovementSpeed < 0f;

        void ApplyForceFromRay(SuspensionRay ray, Rigidbody rigidbody, Vector3 bodyForwardAxis, float forcePerRay) {
            if (!ray.hasCollision) {
                return;
            }

            float num = Mathf.Abs(Mathf.Acos(ray.rayHit.normal.normalized.y));

            if (num < (float)Math.PI / 3f) {
                float num2 = forcePerRay;

                if (num > (float)Math.PI / 3f) {
                    num2 *= ((float)Math.PI / 3f - num) / ((float)Math.PI / 12f);
                }

                Vector3 force = bodyForwardAxis * num2;
                rigidbody.AddForceAtPosition(force, ray.GetGlobalOrigin());
            }
        }

        float RecalculateRelativeTurnSpeed(ChassisNode node, float dt, float maxTurnSpeed, float relativeTurnSpeed,
            float slopeCoeff) {
            ChassisComponent chassis = node.chassis;
            ChassisConfigComponent chassisConfig = node.chassisConfig;
            float num = node.speedConfig.TurnAcceleration * ((float)Math.PI / 180f);
            float num2 = CalculateTurnCoefficient(node.track);
            float num3 = 0f;

            if (chassis.EffectiveTurnAxis == 0f) {
                num3 = (0f - MathUtil.Sign(relativeTurnSpeed)) * num * slopeCoeff * dt;

                if (MathUtil.Sign(relativeTurnSpeed) != MathUtil.Sign(relativeTurnSpeed + num3)) {
                    num3 = 0f - relativeTurnSpeed;
                }
            } else {
                if (IsReversedTurn(chassis.EffectiveTurnAxis, relativeTurnSpeed)) {
                    num = node.speedConfig.ReverseTurnAcceleration * ((float)Math.PI / 180f);
                }

                num3 = chassis.EffectiveTurnAxis * num * slopeCoeff * dt;

                if (chassis.EffectiveMoveAxis == -1f && chassisConfig.ReverseBackTurn) {
                    num3 = 0f - num3;
                }
            }

            return Mathf.Clamp(relativeTurnSpeed + num3, (0f - maxTurnSpeed) * num2, maxTurnSpeed * num2);
        }

        float CalculateTurnCoefficient(TrackComponent trackComponent) {
            float result = 1f;

            if (trackComponent.LeftTrack.numContacts == 0 || trackComponent.RightTrack.numContacts == 0) {
                result = 0.5f;
            }

            return result;
        }

        public class ChassisInitNode : Node {
            public ChassisConfigComponent chassisConfig;

            public DampingComponent damping;
            public RigidbodyComponent rigidbody;

            public SpeedComponent speed;

            public TankCollidersComponent tankColliders;

            public WeightComponent weight;
        }

        [Not(typeof(TankDeadStateComponent))]
        public class ChassisNode : Node {
            public ChassisComponent chassis;

            public ChassisConfigComponent chassisConfig;

            public ChassisSmootherComponent chassisSmoother;

            public EffectiveSpeedComponent effectiveSpeed;

            public RigidbodyComponent rigidbody;

            public SpeedComponent speed;

            public SpeedConfigComponent speedConfig;

            public TankCollidersComponent tankColliders;
            public TankGroupComponent tankGroup;

            public TrackComponent track;
        }

        public class TankActiveStateNode : Node {
            public TankActiveStateComponent tankActiveState;
            public TrackComponent track;
        }
    }
}