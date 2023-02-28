using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636301946304873717L)]
    public interface EmergencyProtectionEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        EmergencyProtectionEffectComponent emergencyProtectionEffect();
    }
}