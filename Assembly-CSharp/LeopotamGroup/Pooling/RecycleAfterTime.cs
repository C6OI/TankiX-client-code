using UnityEngine;

namespace LeopotamGroup.Pooling {
    public sealed class RecycleAfterTime : MonoBehaviour {
        [SerializeField] float _timeout = 1f;

        float _endTime;

        public float Timeout {
            get => _timeout;
            set => _timeout = value;
        }

        void LateUpdate() {
            if (Time.time >= _endTime) {
                OnRecycle();
            }
        }

        void OnEnable() {
            _endTime = Time.time + _timeout;
        }

        void OnRecycle() {
            IPoolObject component = GetComponent<IPoolObject>();

            if (component != null) {
                component.PoolRecycle();
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}