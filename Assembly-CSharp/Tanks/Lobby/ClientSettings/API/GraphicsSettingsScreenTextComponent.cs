using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.API {
    public class GraphicsSettingsScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] TextMeshProUGUI reloadText;

        [SerializeField] TextMeshProUGUI perfomanceChangeText;

        [SerializeField] TextMeshProUGUI currentPerfomanceText;

        [SerializeField] TextMeshProUGUI windowModeText;

        [SerializeField] TextMeshProUGUI resolutionText;

        [SerializeField] TextMeshProUGUI qualityText;

        [SerializeField] TextMeshProUGUI saturationLevelText;

        [SerializeField] TextMeshProUGUI renderResolutionQualityText;

        [SerializeField] TextMeshProUGUI antialiasingQualityText;

        [SerializeField] TextMeshProUGUI textureQualityText;

        [SerializeField] TextMeshProUGUI shadowQualityText;

        [SerializeField] TextMeshProUGUI particleQualityText;

        [SerializeField] TextMeshProUGUI anisotropicQualityText;

        [SerializeField] TextMeshProUGUI customSettingsModeText;

        [SerializeField] TextMeshProUGUI ambientOccluisonModeText;

        [SerializeField] TextMeshProUGUI bloomModeText;

        [SerializeField] TextMeshProUGUI chromaticAberrationModeText;

        [SerializeField] TextMeshProUGUI grainModeText;

        [SerializeField] TextMeshProUGUI vignetteModeText;

        [SerializeField] TextMeshProUGUI vegetationQualityText;

        [SerializeField] TextMeshProUGUI grassQualityText;

        [SerializeField] TextMeshProUGUI cartridgeCaseAmountText;

        [SerializeField] TextMeshProUGUI vSyncQualityText;

        public string ReloadText {
            set => reloadText.text = value;
        }

        public string PerfomanceChangeText {
            set => perfomanceChangeText.text = value;
        }

        public string CurrentPerfomanceText {
            set => currentPerfomanceText.text = value;
        }

        public string WindowModeText {
            set => windowModeText.text = value;
        }

        public string ScreenResolutionText {
            set => resolutionText.text = value;
        }

        public string QualityLevelText {
            set => qualityText.text = value;
        }

        public string SaturationLevelText {
            set => saturationLevelText.text = value;
        }

        public string RenderResolutionQualityText {
            set => renderResolutionQualityText.text = value;
        }

        public string AntialiasingQualityText {
            set => antialiasingQualityText.text = value;
        }

        public string TextureQualityText {
            set => textureQualityText.text = value;
        }

        public string ShadowQualityText {
            set => shadowQualityText.text = value;
        }

        public string ParticleQualityText {
            set => particleQualityText.text = value;
        }

        public string AnisotropicQualityText {
            set => anisotropicQualityText.text = value;
        }

        public string CustomSettingsModeText {
            set => customSettingsModeText.text = value;
        }

        public string AmbientOccluisonModeText {
            set => ambientOccluisonModeText.text = value;
        }

        public string BloomModeText {
            set => bloomModeText.text = value;
        }

        public string ChromaticAberrationModeText {
            set => chromaticAberrationModeText.text = value;
        }

        public string GrainModeText {
            set => grainModeText.text = value;
        }

        public string VignetteModeText {
            set => vignetteModeText.text = value;
        }

        public string VegetationQualityText {
            set => vegetationQualityText.text = value;
        }

        public string GrassQualityText {
            set => grassQualityText.text = value;
        }

        public string CartridgeCaseAmountText {
            set => cartridgeCaseAmountText.text = value;
        }

        public string VSyncQualityText {
            set => vSyncQualityText.text = value;
        }
    }
}