using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.Impl;

namespace Tanks.Battle.ClientHUD.API {
    [TemplatePart]
    [SerialVersionUID(636213789870146534L)]
    public interface EffectHUDTemplate : EffectBaseTemplate, Template {
        [PersistentConfig]
        EffectIconComponent effectIcon();
    }
}