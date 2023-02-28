using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CartridgeCaseComponent : BehaviourComponent {
        public float lifeTime = 5f;

        bool _selfDestructionStarted;

        Collider collider;

        void OnEnable() {
            collider = GetComponent<Collider>();
        }

        public void StartSelfDestruction() {
            if (!_selfDestructionStarted) {
                _selfDestructionStarted = true;
                Invoke("DestroyCase", lifeTime);
                collider = GetComponent<Collider>();
                collider.isTrigger = false;
            }
        }

        void DestroyCase() {
            gameObject.RecycleObject();
            _selfDestructionStarted = false;
        }
    }
}