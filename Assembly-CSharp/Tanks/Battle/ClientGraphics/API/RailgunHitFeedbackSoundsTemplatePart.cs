using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.API {
    [TemplatePart]
    public interface RailgunHitFeedbackSoundsTemplatePart : RailgunBattleItemTemplate, DiscreteWeaponEnergyTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        HitFeedbackSoundsPlayingSettingsComponent hitFeedbackSoundsPlayingSettings();
    }
}