using UnityEngine;

namespace LeopotamGroup.Pooling {
    public interface IPoolObject {
        PoolContainer PoolContainer { get; set; }

        Transform PoolTransform { get; }

        void PoolRecycle(bool checkForDoubleRecycle = true);
    }
}