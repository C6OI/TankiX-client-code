using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BuyXCrystalsDialogComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI title;

        void Update() {
            if (InputMapping.Cancel) {
                Hide();
            }
        }

        public void Show(bool showTitle = true) {
            MainScreenComponent.Instance.OverrideOnBack(Hide);
            title.gameObject.SetActive(showTitle);
            gameObject.SetActive(true);
            GetComponent<Animator>().SetBool("show", true);
        }

        public void Hide() {
            GetComponent<Animator>().SetBool("show", false);
            gameObject.SetActive(false);
        }
    }
}