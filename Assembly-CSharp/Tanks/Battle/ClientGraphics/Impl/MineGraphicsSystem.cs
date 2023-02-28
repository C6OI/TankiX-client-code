using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MineGraphicsSystem : ECSSystem {
        static readonly float MINE_ACTIVATION_TIME = 1f;

        static readonly Vector4 MINE_ACTIVATION_COLOR = new(1f, 1f, 1f, 1f);

        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void ApplyActivationColor(NodeAddedEvent e, MineRendererNode mine) {
            ApplyActivationColor(mine);
        }

        [OnEventFire]
        public void ApplyActivationColor(NodeAddedEvent e, MineRendererPaintedNode mine) {
            ApplyActivationColor(mine);
        }

        void ApplyActivationColor(MineRendererNode mine) {
            mine.effectRendererGraphics.Renderer.material.SetColor("_Color", MINE_ACTIVATION_COLOR);
        }

        [OnEventFire]
        public void Activation(EffectActivationEvent e, SingleNode<MineEffectComponent> mine) {
            mine.Entity.AddComponent(new MineActivationGraphicsComponent(UnityTime.time));
        }

        [OnEventFire]
        public void ActivationEffect(TimeUpdateEvent e, MineActivationNode mine) {
            MineConfigComponent mineConfig = mine.mineConfig;
            float num = UnityTime.time - mine.mineActivationGraphics.ActivationStartTime;
            float num2 = num / (MINE_ACTIVATION_TIME * 0.5f);

            if (num2 > 1f) {
                num2 = Math.Max(0f, 2f - num2);
            }

            Material material = mine.effectRendererGraphics.Renderer.material;
            material.SetColor("_Color", MINE_ACTIVATION_COLOR);
            material.SetFloat("_ColorLerp", num2);

            if (num > MINE_ACTIVATION_TIME) {
                mine.Entity.RemoveComponent<MineActivationGraphicsComponent>();
            }
        }

        [OnEventFire]
        public void AlphaBlendByDistance(TimeUpdateEvent e, MineNode mine, [JoinByTank] EnemyTankNode tank, [JoinByBattle] SelfTankNode selfTank) {
            if (!mine.Entity.HasComponent<MineActivationGraphicsComponent>()) {
                float w = MineCommonGraphicsSystem.BlendMine(mine.mineConfig, mine.effectInstance, mine.effectRendererGraphics, selfTank.hullInstance);
                Vector4 mINE_ACTIVATION_COLOR = MINE_ACTIVATION_COLOR;
                mINE_ACTIVATION_COLOR.w = w;
                mine.effectRendererGraphics.Renderer.material.SetColor("_Color", mINE_ACTIVATION_COLOR);
            }
        }

        [OnEventFire]
        public void PrepareExplosion(NodeAddedEvent e, MinePrepareExplosionnNode mine) { }

        public class SelfTankNode : Node {
            public HullInstanceComponent hullInstance;

            public SelfTankComponent selfTank;
            public TankComponent tank;
        }

        public class MineRendererNode : Node {
            public EffectRendererGraphicsComponent effectRendererGraphics;
        }

        public class MineRendererPaintedNode : MineRendererNode {
            public EffectPaintedComponent effectPainted;
        }

        public class MineNode : MineRendererPaintedNode {
            public EffectInstanceComponent effectInstance;

            public MineConfigComponent mineConfig;
            public MineEffectComponent mineEffect;
        }

        public class MineActivationNode : MineNode {
            public MineActivationGraphicsComponent mineActivationGraphics;
        }

        public class MinePrepareExplosionnNode : MineNode {
            public MinePrepareExplosionComponent minePrepareExplosion;
        }

        public class EnemyTankNode : Node {
            public EnemyComponent enemy;

            public TankComponent tank;
            public TankGroupComponent tankGroup;
        }
    }
}