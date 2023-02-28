using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class GoBackSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void PlayGoBackSound(GoBackEvent evt, SingleNode<GoBackSoundEffectComponent> effect) {
            effect.component.PlaySoundEffect();
        }
    }
}