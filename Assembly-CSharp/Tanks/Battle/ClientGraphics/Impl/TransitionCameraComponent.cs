using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TransitionCameraComponent : Component {
        public TransitionCameraComponent() {
            P1 = Vector3.zero;
            P2 = Vector3.zero;
            P3 = Vector3.zero;
            P4 = Vector3.zero;
            Speed = 0f;
            Distance = 0f;
        }

        public bool Spawn { get; set; }

        public CameraSaveData CameraSaveData { get; set; }

        public Vector3 P1 { get; set; }

        public Vector3 P2 { get; set; }

        public Vector3 P3 { get; set; }

        public Vector3 P4 { get; set; }

        public AngleValues AngleValuesX { get; set; }

        public AngleValues AngleValuesY { get; set; }

        public float TotalDistance { get; set; }

        public float Acceleration { get; set; }

        public float Speed { get; set; }

        public float Distance { get; set; }

        public bool TransitionComplete { get; set; }

        public void Reset() {
            P1 = Vector3.zero;
            P2 = Vector3.zero;
            P3 = Vector3.zero;
            P4 = Vector3.zero;
            Speed = 0f;
            Distance = 0f;
            Spawn = false;
            CameraSaveData = null;
            TransitionComplete = false;
            TotalDistance = 0f;
            Acceleration = 0f;
            AngleValuesX = null;
            AngleValuesY = null;
        }
    }
}