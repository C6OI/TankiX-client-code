using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TrackDustSystem : ECSSystem {
        const float MAX_WORK_DISTANCE = 30f;

        const int EMISSION_RAY_NUMBER = 2;

        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void InitTrackDustSystem(NodeAddedEvent evt, [Combine] TrackDustInitNode tank, SingleNode<MapDustComponent> mapDust) {
            TrackComponent track = tank.track;
            TrackDustComponent trackDust = tank.trackDust;
            trackDust.leftTrackDustDelay = new float[track.LeftTrack.rays.Length];
            trackDust.rightTrackDustDelay = new float[track.RightTrack.rays.Length];
            CollisionDustBehaviour collisionDustBehaviour = tank.hullInstance.HullInstance.AddComponent<CollisionDustBehaviour>();
            collisionDustBehaviour.mapDust = mapDust.component;

            if (tank.Entity.HasComponent<CollisionDustComponent>()) {
                tank.Entity.GetComponent<CollisionDustComponent>().CollisionDustBehaviour = collisionDustBehaviour;
            } else {
                tank.Entity.AddComponent(new CollisionDustComponent(collisionDustBehaviour));
            }
        }

        [OnEventFire]
        public void TryEmitFromTracks(UpdateEvent evt, TrackDustUpdateNode tankNode, [JoinAll] SingleNode<MapDustComponent> mapDustNode) {
            if (tankNode.cameraVisibleTrigger.IsVisibleAtRange(30f)) {
                TrackComponent track = tankNode.track;
                TrackDustComponent trackDust = tankNode.trackDust;
                ChassisConfigComponent chassisConfig = tankNode.chassisConfig;
                float maxRayLength = chassisConfig.MaxRayLength;
                Track leftTrack = track.LeftTrack;
                Track rightTrack = track.RightTrack;
                MapDustComponent component = mapDustNode.component;
                float[] leftTrackDustDelay = trackDust.leftTrackDustDelay;
                float[] rightTrackDustDelay = trackDust.rightTrackDustDelay;
                int numRaysPerTrack = chassisConfig.NumRaysPerTrack;

                for (int i = 0; i < numRaysPerTrack; i += 2) {
                    TryEmitFromSuspensionRay(maxRayLength, leftTrack, component, leftTrackDustDelay, i);
                    TryEmitFromSuspensionRay(maxRayLength, rightTrack, component, rightTrackDustDelay, i);
                }
            }
        }

        void TryEmitFromSuspensionRay(float maxCompression, Track track, MapDustComponent mapDust, float[] delays, int i) {
            SuspensionRay suspensionRay = track.rays[i];
            float num = delays[i];
            num -= UnityTime.deltaTime;

            if (!suspensionRay.hasCollision) {
                delays[i] = num;
                return;
            }

            RaycastHit rayHit = suspensionRay.rayHit;
            Transform transform = rayHit.transform;
            Vector2 textureCoord = rayHit.textureCoord;
            DustEffectBehaviour effectByTag = mapDust.GetEffectByTag(transform, textureCoord);

            if (effectByTag == null) {
                delays[i] = num;
                return;
            }

            Vector3 point = rayHit.point;

            if (num <= 0f) {
                num = 1f / effectByTag.movementEmissionRate.RandomValue;
                effectByTag.TryEmitParticle(point, suspensionRay.velocity);
                delays[i] = num;
                return;
            }

            if (!suspensionRay.hadPreviousCollision) {
                float num2 = Mathf.Clamp01(suspensionRay.compression / maxCompression);

                if (num2 > effectByTag.landingCompressionThreshold) {
                    Vector3 inheritedVelocity = Vector3.up * (effectByTag.movementSpeedThreshold.max * num2);
                    effectByTag.TryEmitParticle(point, inheritedVelocity);
                }
            }

            delays[i] = num;
        }

        public class TrackDustInitNode : Node {
            public ChassisConfigComponent chassisConfig;

            public HullInstanceComponent hullInstance;
            public TankVisualRootComponent tankVisualRoot;

            public TrackComponent track;

            public TrackDustComponent trackDust;
        }

        public class TrackDustUpdateNode : Node {
            public CameraVisibleTriggerComponent cameraVisibleTrigger;

            public ChassisConfigComponent chassisConfig;
            public TankActiveStateComponent tankActiveState;

            public TrackComponent track;

            public TrackDustComponent trackDust;
        }
    }
}