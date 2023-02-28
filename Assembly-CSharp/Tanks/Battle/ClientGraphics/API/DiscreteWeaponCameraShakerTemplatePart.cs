using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.API {
    [TemplatePart]
    [SerialVersionUID(636268928803011249L)]
    public interface DiscreteWeaponCameraShakerTemplatePart : DiscreteWeaponTemplate, WeaponTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        KickbackCameraShakerConfigComponent kickbackCameraShakerConfig();

        [AutoAdded]
        [PersistentConfig]
        ImpactCameraShakerConfigComponent impactCameraShakerConfig();
    }
}