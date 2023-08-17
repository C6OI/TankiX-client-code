using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientGraphics.API {
    [SerialVersionUID(-1455609729657L)]
    public interface QualitySettingsTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        VegetationSettingsComponent vegetationSettings();

        [AutoAdded]
        [PersistentConfig]
        MaterialsSettingsComponent materialsSettings();

        [AutoAdded]
        [PersistentConfig]
        DecalSettingsComponent decalSettings();

        [AutoAdded]
        [PersistentConfig]
        WaterSettingsComponent waterSettings();

        [PersistentConfig]
        [AutoAdded]
        StreamWeaponSettingsComponent streamWeaponSettings();

        [AutoAdded]
        [PersistentConfig]
        SupplyEffectSettingsComponent supplyEffectSettings();
    }
}