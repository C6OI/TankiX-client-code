using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public class DelayedSelfDestroyBehaviour : MonoBehaviour {
        [SerializeField] float delay;

        float destroyTime;

        public float Delay {
            get => delay;
            set => delay = value;
        }

        void Start() {
            destroyTime = Time.time + delay;
        }

        void Update() {
            if (Time.time > destroyTime) {
                Destroy(gameObject);
            }
        }
    }
}