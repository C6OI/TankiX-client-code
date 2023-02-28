using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Battle.ClientGraphics.API {
    [SerialVersionUID(-1455609729657L)]
    public interface QualitySettingsTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        MaterialsSettingsComponent materialsSettings();

        [AutoAdded]
        [PersistentConfig]
        DecalSettingsComponent decalSettings();

        [AutoAdded]
        [PersistentConfig]
        WaterSettingsComponent waterSettings();

        [AutoAdded]
        [PersistentConfig]
        StreamWeaponSettingsComponent streamWeaponSettings();

        [AutoAdded]
        [PersistentConfig]
        SupplyEffectSettingsComponent supplyEffectSettings();

        [AutoAdded]
        [PersistentConfig]
        FeedbackGraphicsRestrictionsComponent feedbackGraphicsRestrictions();
    }
}