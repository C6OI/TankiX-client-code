using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MovingWheel : Wheel {
        public TrackPoint nearestPoint;

        public Vector3 objOffset;

        public MovingWheel(Transform obj)
            : base(obj) { }

        public MovingWheel(Transform obj, TrackPoint nearestPoint, Vector3 offset)
            : base(obj) {
            this.nearestPoint = nearestPoint;
            objOffset = offset;
        }

        public void UpdateObjPosition(Transform transform, Vector3 position) {
            if (transform == obj.parent) {
                obj.localPosition = position + objOffset;
            } else {
                obj.localPosition = obj.parent.InverseTransformPoint(transform.TransformPoint(position + objOffset));
            }
        }
    }
}