using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class BaseHitExplosionSoundSystem : ECSSystem {
        protected void CreateHitExplosionSoundEffect(Vector3 position, GameObject prefab, float duration) {
            Object obj = Object.Instantiate(prefab, position, Quaternion.identity);
            Object.Destroy(obj, duration);
        }
    }
}