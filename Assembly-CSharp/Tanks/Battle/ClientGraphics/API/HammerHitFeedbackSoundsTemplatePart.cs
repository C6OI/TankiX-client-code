using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.API {
    [TemplatePart]
    public interface HammerHitFeedbackSoundsTemplatePart : HammerBattleItemTemplate, DiscreteWeaponTemplate, WeaponTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        HitFeedbackSoundsPlayingSettingsComponent hitFeedbackSoundsPlayingSettings();
    }
}