using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.Impl {
    public class PostProcessingQualitySystem : ECSSystem {
        static readonly string CUSTOM_SETTINGS_MODE_KEY = "CUSTOM_SETTINGS_MODE";

        static readonly int CUSTOM_SETTINGS_DEFAULT_VALUE;

        static readonly string AMBIENT_OCCLUSION_MODE_KEY = "AMBIENT_OCCLUSION_MODE";

        static readonly int AMBIENT_OCCLUSION_DEFAULT_VALUE;

        static readonly string BLOOM_MODE_KEY = "BLOOM_MODE";

        static readonly int BLOOM_DEFAULT_VALUE;

        static readonly string CHROMATIC_ABERRATION_MODE_KEY = "CHROMATIC_ABERRATION_MODE";

        static readonly int CHROMATIC_ABERRATION_DEFAULT_VALUE;

        static readonly string GRAIN_MODE_KEY = "GRAIN_MODE";

        static readonly int GRAIN_DEFAULT_VALUE;

        static readonly string VIGNETTE_MODE_KEY = "VIGNETTE_MODE";

        static readonly int VIGNETTE_DEFAULT_VALUE;

        static readonly string LOW_RENDER_RESOLUTION_MODE_KEY = "LOW_RENDER_RESOLUTION_MODE";

        static readonly int LOW_RENDER_RESOLUTION_DEFAULT_VALUE;

        static readonly string DISABLED_BATTLE_NOTIFICATIONS_KEY = "DISABLED_BATTLED_NOTIFICATIONS";

        static readonly int DISABLED_BATTLE_NOTIFICATIONS_DEFAULT_VALUE;

        [OnEventFire]
        public void InitPostProcessingQualitySettings(NodeAddedEvent e, SingleNode<PostProcessingQualityVariantComponent> postProcessingQualitySettings) {
            postProcessingQualitySettings.component.CustomSettings = PlayerPrefs.GetInt(CUSTOM_SETTINGS_MODE_KEY, CUSTOM_SETTINGS_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.AmbientOcclusion = PlayerPrefs.GetInt(AMBIENT_OCCLUSION_MODE_KEY, AMBIENT_OCCLUSION_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.Bloom = PlayerPrefs.GetInt(BLOOM_MODE_KEY, BLOOM_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.ChromaticAberration = PlayerPrefs.GetInt(CHROMATIC_ABERRATION_MODE_KEY, CHROMATIC_ABERRATION_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.Grain = PlayerPrefs.GetInt(GRAIN_MODE_KEY, GRAIN_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.Vignette = PlayerPrefs.GetInt(VIGNETTE_MODE_KEY, VIGNETTE_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.DisableBattleNotifications = PlayerPrefs.GetInt(DISABLED_BATTLE_NOTIFICATIONS_KEY, DISABLED_BATTLE_NOTIFICATIONS_DEFAULT_VALUE) > 0;
        }

        [OnEventFire]
        public void PostProcessingQualitySettingsChanged(SettingsChangedEvent<PostProcessingQualityVariantComponent> e, Node any) {
            PlayerPrefs.SetInt(CUSTOM_SETTINGS_MODE_KEY, e.Data.CustomSettings ? 1 : 0);
            PlayerPrefs.SetInt(AMBIENT_OCCLUSION_MODE_KEY, e.Data.AmbientOcclusion ? 1 : 0);
            PlayerPrefs.SetInt(BLOOM_MODE_KEY, e.Data.Bloom ? 1 : 0);
            PlayerPrefs.SetInt(CHROMATIC_ABERRATION_MODE_KEY, e.Data.ChromaticAberration ? 1 : 0);
            PlayerPrefs.SetInt(GRAIN_MODE_KEY, e.Data.Grain ? 1 : 0);
            PlayerPrefs.SetInt(VIGNETTE_MODE_KEY, e.Data.Vignette ? 1 : 0);
            PlayerPrefs.SetInt(DISABLED_BATTLE_NOTIFICATIONS_KEY, e.Data.DisableBattleNotifications ? 1 : 0);
        }
    }
}