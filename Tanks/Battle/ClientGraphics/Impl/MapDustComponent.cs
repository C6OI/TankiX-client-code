using System;
using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapDustComponent : MonoBehaviour, Component {
        const float GRAYSCALE_THRESHOLD = 0.5f;

        const string MASK_2_PROPERTY = "_Mask2";

        const string MASK_3_PROPERTY = "_Mask3";

        public DustEffectBehaviour defaultDust;

        public MapDustEffect[] dust;

        Dictionary<Transform, TargetEffects> effects;

        void Awake() {
            int nameID = Shader.PropertyToID("_Mask2");
            int nameID2 = Shader.PropertyToID("_Mask3");
            effects = new Dictionary<Transform, TargetEffects>();
            MapDustEffect[] array = dust;

            foreach (MapDustEffect mapDustEffect in array) {
                Transform target = mapDustEffect.target;

                if ((bool)target) {
                    Renderer component = target.GetComponent<Renderer>();
                    Material material = component.material;

                    effects.Add(target,
                        new TargetEffects {
                            dustEffects = mapDustEffect.dustEffects,
                            mask2 = material.GetTexture(nameID) as Texture2D,
                            mask3 = material.GetTexture(nameID2) as Texture2D
                        });
                }
            }
        }

        public DustEffectBehaviour GetEffectByTag(Transform target, Vector2 uv) {
            if (target.CompareTag(ClientGraphicsConstants.TERRAIN_TAG)) {
                TargetEffects targetEffects = effects[target];

                if ((bool)targetEffects.mask3 && targetEffects.mask3.GetPixelBilinear(uv.x, uv.y).grayscale > 0.5f) {
                    return targetEffects.dustEffects[2];
                }

                if ((bool)targetEffects.mask2 && targetEffects.mask2.GetPixelBilinear(uv.x, uv.y).grayscale > 0.5f) {
                    return targetEffects.dustEffects[1];
                }

                return targetEffects.dustEffects[0];
            }

            return defaultDust;
        }

        [Serializable]
        public class MapDustEffect {
            public Transform target;

            public DustEffectBehaviour[] dustEffects;
        }

        [Serializable]
        public class TargetEffects {
            public DustEffectBehaviour[] dustEffects;

            public Texture2D mask2;

            public Texture2D mask3;
        }
    }
}