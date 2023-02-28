using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [SkipExceptionOnAddRemove]
    public class MapDustComponent : BehaviourComponent {
        public DustEffectBehaviour DefaultDust;

        public DustEffectBehaviour LowGraphicDust;

        public Quality.QualityLevel QualityForLowDust = Quality.QualityLevel.Minimum;

        public int ParentDeep;

        public MapDustEffect[] Targets;

        readonly Dictionary<Transform, TargetEffects> _effects = new();

        void Start() {
            if (DefaultDust == null) {
                DustEffectBehaviour[] array = FindObjectsOfType<DustEffectBehaviour>();

                foreach (DustEffectBehaviour dustEffectBehaviour in array) {
                    if (dustEffectBehaviour.surface != DustEffectBehaviour.SurfaceType.Concrete) {
                        DefaultDust = dustEffectBehaviour;
                        continue;
                    }

                    DefaultDust = dustEffectBehaviour;
                    break;
                }
            }

            if (LowGraphicDust == null) {
                LowGraphicDust = DefaultDust;
            }

            _effects.Clear();
            MapDustEffect[] targets = Targets;

            foreach (MapDustEffect mapDustEffect in targets) {
                Transform target = mapDustEffect.Target;

                if ((bool)target) {
                    TargetEffects targetEffects = new();
                    MaskToDustEffect[] dustEffects = mapDustEffect.DustEffects;

                    foreach (MaskToDustEffect maskToDustEffect in dustEffects) {
                        targetEffects.GrayscaleRanges.Add(maskToDustEffect.GrayScaleRange);
                        targetEffects.Textures.Add(maskToDustEffect.Mask);
                        targetEffects.DustEffects.Add(maskToDustEffect.EffectBehaviour);
                    }

                    _effects.Add(target, targetEffects);
                }
            }
        }

        public DustEffectBehaviour GetEffectByTag(Transform target, Vector2 uv) {
            int qualityLevel = QualitySettings.GetQualityLevel();

            if (qualityLevel <= (int)QualityForLowDust || !target) {
                return LowGraphicDust;
            }

            int num = 0;
            bool flag = _effects.ContainsKey(target);

            while (!flag && ParentDeep > num) {
                target = target.parent;

                if (!target) {
                    break;
                }

                flag = _effects.ContainsKey(target);
            }

            if (!flag) {
                return DefaultDust;
            }

            int num2 = 0;
            TargetEffects targetEffects = _effects[target];

            foreach (Texture2D texture in targetEffects.Textures) {
                if (!texture) {
                    return targetEffects.DustEffects[num2];
                }

                float grayscale = texture.GetPixelBilinear(uv.x, uv.y).grayscale;
                Vector2 vector = targetEffects.GrayscaleRanges[num2];

                if (grayscale >= vector.x && grayscale <= vector.y) {
                    return targetEffects.DustEffects[num2];
                }

                num2++;
            }

            return DefaultDust;
        }

        [Serializable]
        public class MapDustEffect {
            public Transform Target;

            public MaskToDustEffect[] DustEffects;
        }

        [Serializable]
        public class MaskToDustEffect {
            [Tooltip("Min and Max Grayscale")] public Vector2 GrayScaleRange;

            public Texture2D Mask;

            public DustEffectBehaviour EffectBehaviour;
        }

        class TargetEffects {
            public readonly List<DustEffectBehaviour> DustEffects = new();
            public readonly List<Vector2> GrayscaleRanges = new();

            public readonly List<Texture2D> Textures = new();
        }
    }
}