using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankExplosionSoundSystem : ECSSystem {
        [OnEventFire]
        public void PlayDeathSound(NodeAddedEvent evt, TankExplosionNode tank,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) => tank.tankExplosionSound.Sound.Play();

        public class TankExplosionNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankDeadStateComponent tankDeadState;

            public TankExplosionSoundComponent tankExplosionSound;
        }
    }
}