using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RFX4_StartDelay : MonoBehaviour {
        public GameObject ActivatedGameObject;

        public float Delay = 1f;

        void OnEnable() {
            ActivatedGameObject.SetActive(false);
            Invoke("ActivateGO", Delay);
        }

        void OnDisable() {
            CancelInvoke("ActivateGO");
        }

        void ActivateGO() {
            ActivatedGameObject.SetActive(true);
        }
    }
}