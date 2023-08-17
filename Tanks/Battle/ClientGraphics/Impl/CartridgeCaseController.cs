using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CartridgeCaseController : MonoBehaviour {
        public float lifeTime = 5f;

        Collider collider;

        void Start() {
            Invoke("DestroyCase", lifeTime);
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        void OnTriggerExit() => collider.isTrigger = false;

        void DestroyCase() => Destroy(gameObject);
    }
}