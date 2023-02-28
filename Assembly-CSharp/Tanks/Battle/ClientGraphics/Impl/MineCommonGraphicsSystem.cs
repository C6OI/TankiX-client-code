using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MineCommonGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void Explosion(MineExplosionEvent e, MineExplosionNode mine) {
            ModuleEffectGraphicsSystem.InstantiateEffectEffect(mine.effectInstance, mine.mineExplosionGraphics.EffectPrefab, mine.mineExplosionGraphics.ExplosionLifeTime,
                mine.mineExplosionGraphics.Origin);
        }

        public static float BlendMine(MineConfigComponent config, EffectInstanceComponent effectInstance, EffectRendererGraphicsComponent effectRendererGraphics,
            HullInstanceComponent selfTankHullInstance) {
            float num = 1f;
            Vector3 position = effectInstance.GameObject.transform.position;
            Vector3 position2 = selfTankHullInstance.HullInstance.transform.position;
            float magnitude = (position2 - position).magnitude;

            if (magnitude > config.BeginHideDistance) {
                num = 1f - Math.Min(1f, (magnitude - config.BeginHideDistance) / config.HideRange);
            }

            Renderer renderer = effectRendererGraphics.Renderer;
            renderer.enabled = num > 0f;
            return num;
        }

        public class MineExplosionNode : Node {
            public EffectInstanceComponent effectInstance;
            public MineExplosionGraphicsComponent mineExplosionGraphics;
        }
    }
}