using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageModulesScreenComponent : BehaviourComponent {
        [SerializeField] GameObject modulesListItemPrefab;

        [SerializeField] GameObject resourcePriceLabelPrefab;

        [SerializeField] RectTransform modulesListRoot;

        [SerializeField] Text placeholderText;

        [SerializeField] Animator fadable;

        public GameObject ModulesListItemPrefab {
            get => modulesListItemPrefab;
            set => modulesListItemPrefab = value;
        }

        public GameObject ResourcePriceLabelPrefab {
            get => resourcePriceLabelPrefab;
            set => resourcePriceLabelPrefab = value;
        }

        public Text PlaceholderText => placeholderText;

        public RectTransform ModulesListRoot {
            get => modulesListRoot;
            set => modulesListRoot = value;
        }

        public void FadeOut() {
            fadable.SetBool("Visible", false);
        }

        public void FadeIn() {
            fadable.SetBool("Visible", true);
        }
    }
}