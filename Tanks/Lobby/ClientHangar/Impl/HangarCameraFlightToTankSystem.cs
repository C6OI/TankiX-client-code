using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraFlightToTankSystem : ECSSystem {
        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void CalculateFlightToTank(NodeAddedEvent e, ScreenNode screen,
            [JoinAll] HangarCameraLocationViewNode hangar) {
            HangarConfigComponent hangarConfig = hangar.hangarConfig;
            Vector3 position = hangar.camera.Camera.transform.position;
            Vector3 position2 = hangar.hangarTankPosition.transform.position;
            position2.y = position.y;
            Vector3 vector = position - position2;
            vector.y = 0f;
            vector.Normalize();
            Vector3 position3 = hangar.hangarCameraStartPosition.transform.position;
            position3.y = position.y;
            float num = Vector3.Distance(position2, position3);
            float num2 = Vector3.Distance(position2, position);
            float num3 = Mathf.Asin(hangarConfig.FlightToTankRadius / num2) * 2f;
            Vector3 vector2 = Quaternion.Euler(0f, num3 * 57.29578f, 0f) * vector;
            vector2.Normalize();
            Vector3 normalized = ((vector + vector2) / 2f).normalized;
            Vector3 arcFlightPivotPoint = normalized * (num2 / Mathf.Cos(num3 / 2f));
            arcFlightPivotPoint.y = position.y;
            float num4 = num2 * Mathf.Tan(num3 / 2f);
            float num5 = (float)Math.PI + num3;
            float num6 = num4 * num5;
            Vector3 vector3 = position2 + vector2 * num;
            Vector3 vector4 = position2 + vector2 * num2;
            float num7 = Vector3.Distance(vector4, vector3);
            vector3.y = hangar.hangarCameraStartPosition.transform.position.y;
            float num8 = hangarConfig.FlightToTankTime / (num6 + num7);

            Quaternion quaternion =
                Quaternion.LookRotation(hangar.hangarTankPosition.transform.position - vector3, Vector3.up);

            Quaternion middleCameraRotation = Quaternion.Lerp(hangar.camera.Camera.transform.rotation, quaternion, 0.5f);
            HangarCameraFlightDataComponent hangarCameraFlightDataComponent = new();
            hangarCameraFlightDataComponent.FlightTime = hangarConfig.FlightToTankTime;
            hangarCameraFlightDataComponent.ArcFlightPivotPoint = arcFlightPivotPoint;
            hangarCameraFlightDataComponent.ArcFlightTime = num6 * num8;

            hangarCameraFlightDataComponent.ArcFlightAngleSpeed =
                num5 * 57.29578f / hangarCameraFlightDataComponent.ArcFlightTime;

            hangarCameraFlightDataComponent.ArcToLinearPoint = vector4;
            hangarCameraFlightDataComponent.LinearFlightTime = num7 * num8;
            hangarCameraFlightDataComponent.OriginCameraRotation = hangar.camera.Camera.transform.rotation;
            hangarCameraFlightDataComponent.OriginCameraPosition = hangar.camera.Camera.transform.position;
            hangarCameraFlightDataComponent.MiddleCameraRotation = middleCameraRotation;
            hangarCameraFlightDataComponent.DestinationCameraPosition = vector3;
            hangarCameraFlightDataComponent.DestinationCameraRotation = quaternion;
            hangarCameraFlightDataComponent.StartFlightTime = UnityTime.time;
            hangar.Entity.AddComponent(hangarCameraFlightDataComponent);
            hangar.hangarCameraViewState.Esm.ChangeState<HangarCameraViewState.FlightToTankState>();
        }

        [OnEventFire]
        public void SwitchToLocationView(HangarCameraStopFlightEvent e, HangarCameraFlightToTankNode hangar) =>
            hangar.hangarCameraViewState.Esm.ChangeState<HangarCameraViewState.TankViewState>();

        [OnEventComplete]
        public void UpdateCameraHight(UpdateEvent e, HangarCameraFlightToTankNode hangar) {
            HangarConfigComponent hangarConfig = hangar.hangarConfig;
            Vector3 position = hangar.camera.Camera.transform.position;

            position.y = Vector3.Lerp(hangar.hangarCameraFlightData.OriginCameraPosition,
                hangar.hangarCameraFlightData.DestinationCameraPosition,
                (UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime) / hangarConfig.FlightToTankTime).y;

            hangar.camera.Camera.transform.position = position;
            ScheduleEvent<HangarCameraRotateToDestinationEvent>(hangar);
        }

        public class HangarCameraNode : Node {
            public CameraComponent camera;
            public HangarComponent hangar;

            public HangarCameraComponent hangarCamera;

            public HangarCameraStartPositionComponent hangarCameraStartPosition;

            public HangarCameraViewStateComponent hangarCameraViewState;

            public HangarConfigComponent hangarConfig;

            public HangarTankPositionComponent hangarTankPosition;
        }

        public class HangarCameraLocationViewNode : HangarCameraNode {
            public HangarCameraLocationViewComponent hangarCameraLocationView;
        }

        public class HangarCameraFlightToTankNode : HangarCameraNode {
            public HangarCameraFlightDataComponent hangarCameraFlightData;

            public HangarCameraFlightToTankComponent hangarCameraFlightToTank;
        }

        [Not(typeof(HangarCameraKeepLocationComponent))]
        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
        }
    }
}