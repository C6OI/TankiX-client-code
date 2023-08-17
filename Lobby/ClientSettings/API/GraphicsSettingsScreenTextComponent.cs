using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientSettings.API {
    public class GraphicsSettingsScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] Text reloadText;

        [SerializeField] Text perfomanceChangeText;

        [SerializeField] Text currentPerfomanceText;

        [SerializeField] Text windowModeText;

        [SerializeField] Text resolutionText;

        [SerializeField] Text qualityText;

        [SerializeField] Text saturationLevelText;

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
    }
}