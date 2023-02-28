using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ElementsToChangeWhenRentedTankComponent : BehaviourComponent {
        [SerializeField] GameObject[] elementsToHide;

        [SerializeField] Button[] buttonsToDeactivate;

        [SerializeField] GameObject[] elementsToActivate;

        public void SetScreenToRentedTankState() {
            GameObject[] array = elementsToHide;

            foreach (GameObject gameObject in array) {
                gameObject.SetActive(false);
            }

            Button[] array2 = buttonsToDeactivate;

            foreach (Button button in array2) {
                button.interactable = false;
            }

            GameObject[] array3 = elementsToActivate;

            foreach (GameObject gameObject2 in array3) {
                gameObject2.SetActive(true);
            }
        }

        public void ReturnScreenToNormalState() {
            GameObject[] array = elementsToHide;

            foreach (GameObject gameObject in array) {
                gameObject.SetActive(true);
            }

            Button[] array2 = buttonsToDeactivate;

            foreach (Button button in array2) {
                button.interactable = true;
            }

            GameObject[] array3 = elementsToActivate;

            foreach (GameObject gameObject2 in array3) {
                gameObject2.SetActive(false);
            }
        }
    }
}