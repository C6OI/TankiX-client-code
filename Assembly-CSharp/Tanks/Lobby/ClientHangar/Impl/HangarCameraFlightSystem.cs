using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraFlightSystem : ECSSystem {
        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void Init(NodeAddedEvent e, HangarCameraNode hangar) {
            SetupCameraFlightESM(hangar.Entity);
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            NewEvent<HangarCameraArcToLinearFlightEvent>().Attach(hangar).ScheduleDelayed(hangarCameraFlightData.ArcFlightTime);
            NewEvent<HangarCameraStopFlightEvent>().Attach(hangar).ScheduleDelayed(hangarCameraFlightData.FlightTime);
        }

        [OnEventFire]
        public void StopRotateToDestination(NodeRemoveEvent e, HangarCameraNode hangar) {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            hangar.cameraRootTransform.Root.SetRotationSafe(hangarCameraFlightData.DestinationCameraRotation);
        }

        [OnEventFire]
        public void Deinit(NodeRemoveEvent e, HangarCameraInitNode hangar) {
            ScheduleEvent<HangarCameraStopFlightEvent>(hangar);
        }

        void SetupCameraFlightESM(Entity camera) {
            HangarCameraFlightStateComponent hangarCameraFlightStateComponent = new();
            camera.AddComponent(hangarCameraFlightStateComponent);
            EntityStateMachine esm = hangarCameraFlightStateComponent.Esm;
            esm.AddState<HangarCameraFlightState.EmptyState>();
            esm.AddState<HangarCameraFlightState.ArcFlightState>();
            esm.AddState<HangarCameraFlightState.LinearFlightState>();
            esm.ChangeState<HangarCameraFlightState.ArcFlightState>();
        }

        [OnEventFire]
        public void ArcFlight(TimeUpdateEvent e, HangarCameraArcFlightNode hangar) {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            hangar.cameraRootTransform.Root.RotateAround(hangarCameraFlightData.ArcFlightPivotPoint, Vector3.up, e.DeltaTime * hangarCameraFlightData.ArcFlightAngleSpeed);
        }

        [OnEventFire]
        public void SwitchToLinearFlight(HangarCameraArcToLinearFlightEvent e, HangarCameraArcFlightNode hangar) {
            hangar.hangarCameraFlightState.Esm.ChangeState<HangarCameraFlightState.LinearFlightState>();
        }

        [OnEventFire]
        public void StartLinearFlight(NodeAddedEvent e, HangarCameraLinearFlightNode hangar) {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            hangarCameraFlightData.ArcToLinearPoint = hangar.cameraRootTransform.Root.position;
        }

        [OnEventFire]
        public void LinearFlight(TimeUpdateEvent e, HangarCameraLinearFlightNode hangar) {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            float f = Mathf.Clamp01((UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime - hangarCameraFlightData.ArcFlightTime) / hangarCameraFlightData.LinearFlightTime);
            f = Mathf.Pow(f, 1f / 3f);
            hangar.cameraRootTransform.Root.position = Vector3.Lerp(hangarCameraFlightData.ArcToLinearPoint, hangarCameraFlightData.DestinationCameraPosition, f);
        }

        [OnEventFire]
        public void StopLinearFlight(NodeRemoveEvent e, HangarCameraLinearFlightNode hangar) {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            hangar.cameraRootTransform.Root.position = hangarCameraFlightData.DestinationCameraPosition;
        }

        [OnEventFire]
        public void RotateToDestination(HangarCameraRotateToDestinationEvent e, HangarCameraNode hangar) {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            float f = (UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime) / hangarCameraFlightData.FlightTime;
            f = Mathf.Pow(f, 1f / 3f);
            Quaternion a;
            Quaternion b;

            if (f < 0.5) {
                a = hangarCameraFlightData.OriginCameraRotation;
                b = hangarCameraFlightData.MiddleCameraRotation;
                f *= 2f;
            } else {
                a = hangarCameraFlightData.MiddleCameraRotation;
                b = hangarCameraFlightData.DestinationCameraRotation;
                f = (f - 0.5f) * 2f;
            }

            hangar.cameraRootTransform.Root.SetRotationSafe(Quaternion.Slerp(a, b, f));
        }

        [OnEventComplete]
        public void StopFlight(HangarCameraStopFlightEvent e, HangarCameraFlightNode hangar) {
            hangar.hangarCameraFlightState.Esm.ChangeState<HangarCameraFlightState.EmptyState>();
            hangar.Entity.RemoveComponent<HangarCameraFlightDataComponent>();
            hangar.Entity.RemoveComponent<HangarCameraFlightStateComponent>();
        }

        public class HangarCameraInitNode : Node {
            public CameraComponent camera;

            public CameraRootTransformComponent cameraRootTransform;
            public HangarComponent hangar;

            public HangarCameraComponent hangarCamera;

            public HangarConfigComponent hangarConfig;

            public HangarTankPositionComponent hangarTankPosition;
        }

        public class HangarCameraNode : HangarCameraInitNode {
            public HangarCameraFlightDataComponent hangarCameraFlightData;
        }

        public class HangarCameraArcFlightNode : HangarCameraNode {
            public HangarCameraArcFlightComponent hangarCameraArcFlight;
            public HangarCameraFlightStateComponent hangarCameraFlightState;
        }

        public class HangarCameraLinearFlightNode : HangarCameraNode {
            public HangarCameraFlightStateComponent hangarCameraFlightState;

            public HangarCameraLinearFlightComponent hangarCameraLinearFlight;
        }

        public class HangarCameraFlightNode : HangarCameraNode {
            public HangarCameraFlightStateComponent hangarCameraFlightState;
        }
    }
}