using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636362347202743953L)]
    public interface EmergencyProtectionHealingPartEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        EmergencyProtectionHealingPartEffectComponent emergencyProtectionHealingPartEffect();
    }
}