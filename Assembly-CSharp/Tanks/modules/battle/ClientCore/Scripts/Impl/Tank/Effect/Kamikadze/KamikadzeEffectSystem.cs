using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.Impl;

namespace tanks.modules.battle.ClientCore.Scripts.Impl.Tank.Effect.Kamikadze {
    public class KamikadzeEffectSystem : ECSSystem {
        [OnEventFire]
        public void EnableEffect(SelfTankExplosionEvent e, SelfTankNode selfTank, [JoinByTank] KamikadzeEffectNode effectNode) {
            ScheduleEvent<StartSplashEffectEvent>(effectNode);
        }

        public class KamikadzeEffectNode : Node {
            public KamikadzeEffectComponent kamikadzeEffect;
        }

        public class SelfTankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public ModuleVisualEffectObjectsComponent moduleVisualEffectObjects;

            public RigidbodyComponent rigidbody;
            public SelfTankComponent selfTank;

            public TankActiveStateComponent tankActiveState;
        }
    }
}