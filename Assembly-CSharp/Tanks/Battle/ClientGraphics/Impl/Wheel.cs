using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class Wheel {
        public Transform obj;
        public float radius;

        float rotation;

        public Wheel(Transform obj) => this.obj = obj;

        public void SetRotation(float angle) {
            obj.localRotation *= Quaternion.AngleAxis(angle - rotation, Vector3.right);
            rotation = angle;
        }
    }
}