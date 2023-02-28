using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1487238139175L)]
    public interface TargetFocusEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        TargetFocusEffectComponent targetFocusEffect();

        [AutoAdded]
        [PersistentConfig]
        TargetFocusVerticalTargetingComponent targetFocusVerticalTargeting();

        [AutoAdded]
        [PersistentConfig]
        TargetFocusVerticalSectorTargetingComponent targetFocusVerticalSectorTargeting();

        [AutoAdded]
        [PersistentConfig]
        TargetFocusConicTargetingComponent targetFocusConicTargeting();

        [AutoAdded]
        [PersistentConfig]
        TargetFocusPelletConeComponent targetFocusPelletCone();
    }
}