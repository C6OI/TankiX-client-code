using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectLazyLoad : MonoBehaviour {
        public GameObject GO;

        public float TimeDelay = 0.3f;

        void Awake() => GO.SetActive(false);

        void OnEnable() => Invoke("LazyEnable", TimeDelay);

        void OnDisable() {
            CancelInvoke("LazyEnable");
            GO.SetActive(false);
        }

        void LazyEnable() => GO.SetActive(true);
    }
}