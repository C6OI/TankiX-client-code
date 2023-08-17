using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraFlightSystem : ECSSystem {
        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void Init(NodeAddedEvent e, HangarCameraNode hangar) {
            SetupCameraFlightESM(hangar.Entity);
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;

            NewEvent<HangarCameraArcToLinearFlightEvent>().Attach(hangar)
                .ScheduleDelayed(hangarCameraFlightData.ArcFlightTime);

            NewEvent<HangarCameraStopFlightEvent>().Attach(hangar).ScheduleDelayed(hangarCameraFlightData.FlightTime);
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
        public void RotateToDestination(HangarCameraRotateToDestinationEvent e, HangarCameraNode hangar) {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;
            float num = (UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime) / hangarCameraFlightData.FlightTime;
            Quaternion a;
            Quaternion b;

            if (num < 0.5) {
                a = hangarCameraFlightData.OriginCameraRotation;
                b = hangarCameraFlightData.MiddleCameraRotation;
                num *= 2f;
            } else {
                a = hangarCameraFlightData.MiddleCameraRotation;
                b = hangarCameraFlightData.DestinationCameraRotation;
                num = (num - 0.5f) * 2f;
            }

            hangar.camera.Camera.transform.rotation = Quaternion.Lerp(a, b, num);
        }

        [OnEventFire]
        public void ArcFlight(TimeUpdateEvent e, HangarCameraArcFlightNode hangar) {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;

            hangar.camera.Camera.transform.RotateAround(hangarCameraFlightData.ArcFlightPivotPoint,
                Vector3.up,
                e.DeltaTime * hangarCameraFlightData.ArcFlightAngleSpeed);
        }

        [OnEventFire]
        public void SwitchToLinearFlight(HangarCameraArcToLinearFlightEvent e, HangarCameraArcFlightNode hangar) =>
            hangar.hangarCameraFlightState.Esm.ChangeState<HangarCameraFlightState.LinearFlightState>();

        [OnEventFire]
        public void LinearFlight(TimeUpdateEvent e, HangarCameraLinearFlightNode hangar) {
            HangarCameraFlightDataComponent hangarCameraFlightData = hangar.hangarCameraFlightData;

            hangar.camera.Camera.transform.position = Vector3.Lerp(hangarCameraFlightData.ArcToLinearPoint,
                hangarCameraFlightData.DestinationCameraPosition,
                (UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime - hangarCameraFlightData.ArcFlightTime) /
                hangarCameraFlightData.LinearFlightTime);
        }

        [OnEventFire]
        public void StopFlight(HangarCameraStopFlightEvent e, HangarCameraFlightNode hangar) {
            hangar.hangarCameraFlightState.Esm.ChangeState<HangarCameraFlightState.EmptyState>();
            hangar.Entity.RemoveComponent<HangarCameraFlightDataComponent>();
            hangar.Entity.RemoveComponent<HangarCameraFlightStateComponent>();
        }

        public class HangarCameraNode : Node {
            public CameraComponent camera;
            public HangarComponent hangar;

            public HangarCameraComponent hangarCamera;

            public HangarCameraFlightDataComponent hangarCameraFlightData;

            public HangarConfigComponent hangarConfig;

            public HangarTankPositionComponent hangarTankPosition;
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
            public new HangarCameraFlightDataComponent hangarCameraFlightData;
            public HangarCameraFlightStateComponent hangarCameraFlightState;
        }
    }
}