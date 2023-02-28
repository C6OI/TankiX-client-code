using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientSettings.API {
    [SerialVersionUID(635824351226675226L)]
    public class SelectLocaleScreenComponent : MonoBehaviour, Component {
        [SerializeField] GameObject applyButton;

        [SerializeField] GameObject cancelButton;

        public void EnableButtons() {
            applyButton.SetActive(true);
        }

        public void DisableButtons() {
            if (applyButton.activeSelf) {
                applyButton.SetActive(false);
                cancelButton.SetActive(false);
            }
        }
    }
}