using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DoubleDamageEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitDoubleDamageEffect(NodeAddedEvent e, [Combine] InitialWeaponNode weapon, [Combine] [JoinByTank] TankNode tank,
            SingleNode<SoundListenerBattleStateComponent> soundListener, SingleNode<SupplyEffectSettingsComponent> settings) {
            if (!tank.Entity.HasComponent<TankDeadStateComponent>()) {
                weapon.doubleDamageEffect.InitEffect(settings.component);
                weapon.Entity.AddComponent<DoubleDamageEffectReadyComponent>();
            }
        }

        [OnEventFire]
        public void Reset(NodeAddedEvent e, TankIncarnationNode incarnation, [JoinByTank] InitializedWeaponNode weapon) {
            weapon.doubleDamageEffect.Reset();
        }

        [OnEventFire]
        public void PlayDoubleDamageEffect(NodeAddedEvent e, DamageEffectNode effect, [Context] [JoinByTank] ReadyWeaponNode weapon) {
            weapon.doubleDamageEffect.Play();
        }

        [OnEventFire]
        public void StopDoubleDamageEffect(NodeRemoveEvent e, DamageEffectNode effect, [JoinByTank] ReadyWeaponNode weapon) {
            weapon.doubleDamageEffect.Stop();
        }

        public class DamageEffectNode : Node {
            public DamageEffectComponent damageEffect;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public TankGroupComponent tankGroup;
        }

        public class InitialWeaponNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public DoubleDamageEffectComponent doubleDamageEffect;

            public TankGroupComponent tankGroup;
        }

        public class ReadyWeaponNode : InitialWeaponNode {
            public DoubleDamageEffectReadyComponent doubleDamageEffectReady;
        }

        public class InitializedWeaponNode : InitialWeaponNode {
            public DoubleDamageEffectReadyComponent doubleDamageEffectReady;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }
    }
}