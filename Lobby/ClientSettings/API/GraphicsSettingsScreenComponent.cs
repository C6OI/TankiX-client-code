using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientSettings.API {
    public class GraphicsSettingsScreenComponent : BehaviourComponent {
        [SerializeField] GameObject applyButton;

        [SerializeField] GameObject cancelButton;

        [SerializeField] GameObject defaultButton;

        [SerializeField] Text reloadText;

        [SerializeField] Text perfomanceChangeText;

        [SerializeField] Text currentPerfomanceText;

        bool needToReloadApplication;

        public bool NeedToReloadApplication {
            get => needToReloadApplication;
            set {
                needToReloadApplication = value;
                reloadText.gameObject.SetActive(needToReloadApplication);
            }
        }

        public void SetPerfomanceWarningVisibility(bool needToShowChangePerfomance, bool isCurrentQuality) {
            perfomanceChangeText.gameObject.SetActive(!isCurrentQuality && needToShowChangePerfomance);
            currentPerfomanceText.gameObject.SetActive(isCurrentQuality && needToShowChangePerfomance);
        }

        public void SetVisibilityForChangeSettingsControls(bool needToShowReload, bool needToShowButtons) {
            applyButton.gameObject.SetActive(needToShowButtons);
            cancelButton.gameObject.SetActive(needToShowButtons);
            NeedToReloadApplication = needToShowReload;
        }

        public void SetDefaultButtonVisibility(bool needToShow) => defaultButton.gameObject.SetActive(needToShow);
    }
}