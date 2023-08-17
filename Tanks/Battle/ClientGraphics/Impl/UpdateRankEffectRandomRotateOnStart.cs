using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectRandomRotateOnStart : MonoBehaviour {
        public Vector3 NormalizedRotateVector = new(0f, 1f, 0f);

        bool isInitialized;

        Transform t;

        void Start() {
            t = transform;
            t.Rotate(NormalizedRotateVector * Random.Range(0, 360));
            isInitialized = true;
        }

        void OnEnable() {
            if (isInitialized) {
                t.Rotate(NormalizedRotateVector * Random.Range(0, 360));
            }
        }
    }
}