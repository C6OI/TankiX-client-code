using UnityEngine;

namespace LeopotamGroup.Pooling {
    public class PoolObject : MonoBehaviour, IPoolObject {
        PoolContainer _container;

        public virtual PoolContainer PoolContainer {
            get => _container;
            set => _container = value;
        }

        public virtual Transform PoolTransform => transform;

        public virtual void PoolRecycle(bool checkDoubleRecycles = true) {
            if ((object)_container != null) {
                _container.Recycle(this, checkDoubleRecycles);
            }
        }
    }
}