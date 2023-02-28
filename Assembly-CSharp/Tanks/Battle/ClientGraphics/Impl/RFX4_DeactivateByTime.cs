using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RFX4_DeactivateByTime : MonoBehaviour {
        public float DeactivateTime = 3f;

        bool canUpdateState;

        void Update() {
            if (canUpdateState) {
                canUpdateState = false;
                Invoke("DeactivateThis", DeactivateTime);
            }
        }

        void OnEnable() {
            canUpdateState = true;
        }

        void DeactivateThis() {
            gameObject.SetActive(false);
        }
    }
}