using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateUserRankTransformBehaviour : MonoBehaviour {
        void Update() {
            transform.up = Vector3.up;
        }

        public void Init() {
            transform.up = Vector3.up;
        }
    }
}