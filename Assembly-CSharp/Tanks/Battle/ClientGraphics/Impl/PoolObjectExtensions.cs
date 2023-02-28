using LeopotamGroup.Pooling;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public static class PoolObjectExtensions {
        public static void RecycleObject(this GameObject gameObject) {
            if ((bool)gameObject) {
                gameObject.transform.rotation = Quaternion.identity;
                IPoolObject component = gameObject.GetComponent<IPoolObject>();

                if (component != null) {
                    component.PoolRecycle();
                } else {
                    Object.Destroy(gameObject);
                }
            }
        }
    }
}