using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BuyBlueprintsButtonComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI titleText;

        [SerializeField] LocalizedField buyBlueprintButtonLocalizedField;

        public bool mountButtonActive {
            set {
                GetComponent<Button>().interactable = value;
                GetComponent<CanvasGroup>().alpha = !value ? 0.2f : 1f;
            }
        }
    }
}