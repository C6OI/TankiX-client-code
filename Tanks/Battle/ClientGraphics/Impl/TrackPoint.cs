using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TrackPoint {
        public Vector3 currentPosition;

        public float desiredVerticalPosition;
        public Vector3 initialPosition;

        public Transform obj;

        public Vector3 objOffset;

        public float velocity;

        public TrackPoint(Vector3 position, Transform trackPoint, Vector3 trackPointOffset) {
            initialPosition = position;
            currentPosition = new Vector3(position.x, position.y, position.z);
            desiredVerticalPosition = position.y;
            obj = trackPoint;
            objOffset = trackPointOffset;
        }

        public void UpdateTrackPointPosition(Transform transform, Vector3 position) {
            if (transform == obj.parent) {
                obj.localPosition = position + objOffset;
            } else {
                obj.localPosition = obj.parent.InverseTransformPoint(transform.TransformPoint(position + objOffset));
            }
        }
    }
}