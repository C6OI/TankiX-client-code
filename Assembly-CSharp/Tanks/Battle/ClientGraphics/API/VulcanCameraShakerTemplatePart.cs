using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.API {
    [TemplatePart]
    [SerialVersionUID(636268952976123872L)]
    public interface VulcanCameraShakerTemplatePart : VulcanBattleItemTemplate, WeaponTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        KickbackCameraShakerConfigComponent kickbackCameraShakerConfig();

        [AutoAdded]
        [PersistentConfig]
        ImpactCameraShakerConfigComponent impactCameraShakerConfig();
    }
}