using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace tanks.modules.battle.ClientCore.Scripts.Impl.Tank.Effect.Kamikadze {
    [SerialVersionUID(1554279538858L)]
    public interface KamikadzeEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        KamikadzeEffectComponent kamikadzeEffect();
    }
}