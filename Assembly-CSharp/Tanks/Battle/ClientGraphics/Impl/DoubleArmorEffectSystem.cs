using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DoubleArmorEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitDoubleArmorEffect(NodeAddedEvent evt, [Combine] TankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener) {
            if (!tank.Entity.HasComponent<TankDeadStateComponent>()) {
                tank.Entity.AddComponent<DoubleArmorEffectReadyComponent>();
            }
        }

        [OnEventFire]
        public void StartDoubleArmorEffect(NodeAddedEvent evt, ArmorEffectNode effect, [Context] [JoinByTank] ReadyTankNode tank) {
            tank.doubleArmorEffect.Play();
        }

        [OnEventFire]
        public void StopDoubleArmorEffect(NodeRemoveEvent evt, ArmorEffectNode effect, [JoinByTank] ReadyTankNode tank) {
            tank.doubleArmorEffect.Stop();
        }

        public class ArmorEffectNode : Node {
            public ArmorEffectComponent armorEffect;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public DoubleArmorEffectComponent doubleArmorEffect;

            public TankGroupComponent tankGroup;
        }

        public class ReadyTankNode : TankNode {
            public DoubleArmorEffectReadyComponent doubleArmorEffectReady;
        }
    }
}