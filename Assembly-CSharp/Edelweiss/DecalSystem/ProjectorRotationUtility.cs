using UnityEngine;

namespace Edelweiss.DecalSystem {
    public class ProjectorRotationUtility {
        static readonly Quaternion s_RotationOffset = Quaternion.Euler(-90f, 0f, 0f);

        public static Quaternion ProjectorRotation(Vector3 a_ProjectionDirection, Vector3 a_ProjectionUpDirection) {
            Quaternion quaternion = Quaternion.LookRotation(a_ProjectionDirection, a_ProjectionUpDirection);
            return quaternion * s_RotationOffset;
        }
    }
}