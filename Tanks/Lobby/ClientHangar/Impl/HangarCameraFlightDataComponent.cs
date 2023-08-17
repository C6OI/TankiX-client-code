using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraFlightDataComponent : Component {
        public float StartFlightTime { get; set; }

        public float FlightTime { get; set; }

        public Vector3 ArcFlightPivotPoint { get; set; }

        public float ArcFlightAngleSpeed { get; set; }

        public float ArcFlightTime { get; set; }

        public Vector3 ArcToLinearPoint { get; set; }

        public float LinearFlightTime { get; set; }

        public Quaternion OriginCameraRotation { get; set; }

        public Vector3 OriginCameraPosition { get; set; }

        public Quaternion MiddleCameraRotation { get; set; }

        public Quaternion DestinationCameraRotation { get; set; }

        public Vector3 DestinationCameraPosition { get; set; }
    }
}