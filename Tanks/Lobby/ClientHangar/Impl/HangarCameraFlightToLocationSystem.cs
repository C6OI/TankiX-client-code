using System;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientHangar.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraFlightToLocationSystem : ECSSystem {
        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void CalculateFlightToLocation(NodeAddedEvent e, HangarLocationScreenNode screen,
            [JoinAll] HangarCameraTankViewNode hangar) {
            Transform value;

            if (hangar.hangarLocations.Locations.TryGetValue(screen.hangarLocation.HangarLocation, out value)) {
                HangarConfigComponent hangarConfig = hangar.hangarConfig;
                Vector3 position = hangar.camera.Camera.transform.position;
                Vector3 position2 = hangar.hangarTankPosition.transform.position;
                position2.y = position.y;
                Vector3 position3 = value.position;
                position3.y = position.y;
                Vector3 vector = position - position2;
                vector.y = 0f;
                vector.Normalize();
                Vector3 vector2 = position3 - position2;
                vector2.y = 0f;
                vector2.Normalize();
                float num = Vector3.Distance(position2, position);
                float num2 = Vector3.Angle(vector, vector2) * ((float)Math.PI / 180f);
                Vector3 normalized = ((vector + vector2) / 2f).normalized;
                Vector3 vector3 = Vector3.Cross(vector2, vector);
                Vector3 arcFlightPivotPoint = normalized * (num / Mathf.Cos(num2 / 2f));
                float num3 = num * Mathf.Tan(num2 / 2f);
                float num4 = (float)Math.PI - num2;
                float num5 = num3 * num4;
                Vector3 vector4 = position2 + vector2 * num;
                float num6 = Vector3.Distance(vector4, position3);
                float num7 = hangarConfig.FlightToLocationTime / (num5 + num6);

                Vector3 vector5 = Vector3.Cross(hangar.camera.Camera.transform.rotation * Vector3.forward,
                    value.rotation * Vector3.forward);

                Quaternion quaternion = Quaternion.Lerp(hangar.camera.Camera.transform.rotation, value.rotation, 0.5f);

                if (MathUtil.Sign(vector3.y) > 0f && MathUtil.Sign(vector5.y) < 0f) {
                    quaternion = Quaternion.AngleAxis(180f, Vector3.up) * quaternion;
                }

                HangarCameraFlightDataComponent hangarCameraFlightDataComponent = new();
                hangarCameraFlightDataComponent.FlightTime = hangarConfig.FlightToLocationTime;
                hangarCameraFlightDataComponent.ArcFlightPivotPoint = arcFlightPivotPoint;
                hangarCameraFlightDataComponent.ArcFlightTime = num5 * num7;

                hangarCameraFlightDataComponent.ArcFlightAngleSpeed =
                    num4 * 57.29578f / hangarCameraFlightDataComponent.ArcFlightTime * MathUtil.Sign(vector3.y);

                hangarCameraFlightDataComponent.ArcToLinearPoint = vector4;
                hangarCameraFlightDataComponent.LinearFlightTime = num6 * num7;
                hangarCameraFlightDataComponent.OriginCameraRotation = hangar.camera.Camera.transform.rotation;
                hangarCameraFlightDataComponent.OriginCameraPosition = hangar.camera.Camera.transform.position;
                hangarCameraFlightDataComponent.MiddleCameraRotation = quaternion;
                hangarCameraFlightDataComponent.DestinationCameraPosition = value.position;
                hangarCameraFlightDataComponent.DestinationCameraRotation = value.rotation;
                hangarCameraFlightDataComponent.StartFlightTime = UnityTime.time;
                hangar.Entity.AddComponent(hangarCameraFlightDataComponent);
                hangar.hangarCameraViewState.Esm.ChangeState<HangarCameraViewState.FlightToLocationState>();
            }
        }

        [OnEventFire]
        public void SwitchToLocationView(HangarCameraStopFlightEvent e, HangarCameraFlightToLocationNode hangar) =>
            hangar.hangarCameraViewState.Esm.ChangeState<HangarCameraViewState.LocationViewState>();

        [OnEventComplete]
        public void UpdateCameraHight(UpdateEvent e, HangarCameraFlightToLocationNode hangar) {
            HangarConfigComponent hangarConfig = hangar.hangarConfig;
            Vector3 position = hangar.camera.Camera.transform.position;

            position.y = CalculateBezierPoint(
                (UnityTime.time - hangar.hangarCameraFlightData.StartFlightTime) / hangarConfig.FlightToLocationTime,
                hangar.hangarCameraFlightData.OriginCameraPosition,
                hangar.hangarTankPosition.transform.position + Vector3.up * hangarConfig.FlightToLocationHigh,
                hangar.hangarCameraFlightData.DestinationCameraPosition).y;

            hangar.camera.Camera.transform.position = position;
            ScheduleEvent<HangarCameraRotateToDestinationEvent>(hangar);
        }

        Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
            float num = 1f - t;
            return num * num * p0 + 2f * num * t * p1 + t * t * p2;
        }

        public class HangarCameraNode : Node {
            public CameraComponent camera;
            public HangarComponent hangar;

            public HangarCameraComponent hangarCamera;

            public HangarCameraViewStateComponent hangarCameraViewState;

            public HangarConfigComponent hangarConfig;

            public HangarLocationsComponent hangarLocations;

            public HangarTankPositionComponent hangarTankPosition;
        }

        public class HangarCameraTankViewNode : HangarCameraNode {
            public HangarCameraTankViewComponent hangarCameraTankView;
        }

        public class HangarCameraFlightToLocationNode : HangarCameraNode {
            public HangarCameraFlightDataComponent hangarCameraFlightData;

            public HangarCameraFlightToLocationComponent hangarCameraFlightToLocation;
        }

        public class HangarLocationScreenNode : Node {
            public HangarLocationComponent hangarLocation;
            public ScreenComponent screen;
        }
    }
}