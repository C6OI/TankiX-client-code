using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TrackMarksBuilderComponent : Component {
        public bool[] contiguous;

        public WheelData[] currentLeftWheelsData;

        public WheelData[] currentRightWheelsData;

        public Vector3[] directions;
        public Transform[] leftWheels;

        public float moveStep;

        public Vector3[] nextNormals;

        public Vector3[] nextPositions;

        public Vector3[] normals;

        public Vector3[] positions;

        public bool[] prevHits;

        public WheelData[] prevLeftWheelsData;

        public WheelData[] prevRightWheelsData;

        public float[] remaingDistance;

        public bool[] resetWheels;

        public Transform[] rightWheels;

        public Rigidbody rigidbody;

        public float[] side;

        public WheelData[] tempLeftWheelsData;

        public WheelData[] tempRightWheelsData;
    }
}