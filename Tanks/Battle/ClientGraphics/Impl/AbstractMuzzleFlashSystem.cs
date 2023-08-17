using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class AbstractMuzzleFlashSystem : ECSSystem {
        protected void InstantiateMuzzleEffect(GameObject prefab, MuzzlePointComponent muzzlePointComponent,
            float duration) {
            GameObject gameObject = Object.Instantiate(prefab);
            UnityUtil.InheritAndEmplace(gameObject.transform, muzzlePointComponent.Current);
            gameObject.gameObject.SetActive(true);
            Object.Destroy(gameObject, duration);
        }
    }
}