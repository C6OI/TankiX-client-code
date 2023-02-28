using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public static class SupplyEffectUtil {
        public static ParticleSystem[] InstantiateEffect(GameObject effectPrefab, Transform effectPoints) {
            ParticleSystem[] array = new ParticleSystem[effectPoints.childCount];

            for (int i = 0; i < effectPoints.childCount; i++) {
                GameObject gameObject = Object.Instantiate(effectPrefab);
                Transform transform = gameObject.transform;
                transform.SetParent(effectPoints.GetChild(i), false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
                component.Stop(true);
                array[i] = component;
            }

            return array;
        }
    }
}