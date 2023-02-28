using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.API {
    [TemplatePart]
    public interface ThunderHitFeedbackSoundsTemplatePart : ThunderBattleItemTemplate, DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        HitFeedbackSoundsPlayingSettingsComponent hitFeedbackSoundsPlayingSettings();
    }
}