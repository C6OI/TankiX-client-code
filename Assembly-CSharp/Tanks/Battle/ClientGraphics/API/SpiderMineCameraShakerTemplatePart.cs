using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.API {
    [TemplatePart]
    [SerialVersionUID(636270875338927636L)]
    public interface SpiderMineCameraShakerTemplatePart : SpiderEffectTemplate, EffectBaseTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ImpactCameraShakerConfigComponent impactCameraShakerConfig();
    }
}