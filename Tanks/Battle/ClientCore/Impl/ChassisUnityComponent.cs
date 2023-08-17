using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ChassisUnityComponent : MonoBehaviour {
        public float suspensionRayOffsetY = 0.1f;

        public int numRaysPerTrack = 5;

        public float maxRayLength = 0.5f;

        public float nominalRayLength = 0.25f;

        public float NominalRayOuterLength => nominalRayLength - suspensionRayOffsetY;
    }
}