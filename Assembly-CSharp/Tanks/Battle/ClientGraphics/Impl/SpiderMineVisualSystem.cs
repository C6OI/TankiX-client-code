using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SpiderMineVisualSystem : ECSSystem {
        [OnEventFire]
        public void AlfaBlendByDistance(TimeUpdateEvent e, MineBlendNode mine, [JoinByTank] EnemyTankNode isEnemy, [JoinByBattle] SelfTankNode selfTank) {
            mine.effectRendererGraphics.Renderer.material.SetFloat(TankMaterialPropertyNames.ALPHA,
                MineCommonGraphicsSystem.BlendMine(mine.mineConfig, mine.effectInstance, mine.effectRendererGraphics, selfTank.hullInstance));
        }

        public class EnemyTankNode : Node {
            public EnemyComponent enemy;
            public RemoteTankComponent remoteTank;

            public TankGroupComponent tankGroup;
        }

        public class MineBlendNode : Node {
            public EffectInstanceComponent effectInstance;

            public EffectRendererGraphicsComponent effectRendererGraphics;

            public MineConfigComponent mineConfig;
            public SpiderMineEffectComponent spiderMineEffect;
        }

        public class SelfTankNode : Node {
            public HullInstanceComponent hullInstance;
            public SelfTankComponent selfTank;
        }
    }
}