using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class NitroEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitNitroEffect(NodeAddedEvent e, [Combine] InitialTankNode tank,
            SingleNode<SoundListenerBattleStateComponent> soundListener,
            SingleNode<SupplyEffectSettingsComponent> settings) {
            if (!tank.Entity.HasComponent<TankDeadStateComponent>()) {
                tank.nitroEffect.InitEffect(settings.component);
                tank.Entity.AddComponent<NitroEffectReadyComponent>();
            }
        }

        [OnEventFire]
        public void StartNitroEffect(NodeAddedEvent e, SpeedEffectNode effect, [JoinByTank] [Context] ReadyTankNode tank) =>
            tank.nitroEffect.Play();

        [OnEventFire]
        public void StopNitroEffect(NodeRemoveEvent evt, SpeedEffectNode effect, [JoinByTank] ReadyTankNode tank) =>
            tank.nitroEffect.Stop();

        public class SpeedEffectNode : Node {
            public SpeedEffectComponent speedEffect;
            public TankGroupComponent tankGroup;
        }

        public class InitialTankNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public NitroEffectComponent nitroEffect;

            public TankGroupComponent tankGroup;
        }

        public class ReadyTankNode : InitialTankNode {
            public NitroEffectReadyComponent nitroEffectReady;
        }
    }
}