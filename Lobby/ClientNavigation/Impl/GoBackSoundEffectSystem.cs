using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.Impl {
    public class GoBackSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void PlayGoBackSound(GoBackEvent evt, SingleNode<GoBackSoundEffectComponent> effect) =>
            effect.component.PlaySoundEffect();
    }
}