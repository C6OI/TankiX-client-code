using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EnergyInjectionSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void PlayEnergyInjectionSoundEffect(ExecuteEnergyInjectionEvent e, SingleNode<EnergyInjectionEffectComponent> effect, [JoinByTank] SelfTankNode tank) {
            tank.energyInjectionSoundEffect.Sound.StopImmediately();
            tank.energyInjectionSoundEffect.Sound.FadeIn();
        }

        public class SelfTankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public EnergyInjectionSoundEffectComponent energyInjectionSoundEffect;
            public SelfTankComponent selfTank;
        }
    }
}