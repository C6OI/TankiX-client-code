using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectDeadTime : MonoBehaviour {
        public float deadTime = 1.5f;

        public bool destroyRoot;

        void Awake() {
            Destroy(destroyRoot ? transform.root.gameObject : gameObject, deadTime);
        }
    }
}