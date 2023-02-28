using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageListItemContentComponent : LocalizedControl, Component {
        [SerializeField] Text header;

        [SerializeField] Text count;

        [SerializeField] Text proficiencyLevel;

        [SerializeField] ProgressBar progressBar;

        [SerializeField] GameObject priceGameObject;

        [SerializeField] GameObject xPriceGameObject;

        [SerializeField] GameObject upgradeGameObject;

        [SerializeField] GameObject arrow;

        [SerializeField] Graphic unlockGraphic;

        [SerializeField] GameObject previewContainer;

        [SerializeField] GameObject previewPrefab;

        [SerializeField] Text rareText;

        [SerializeField] GameObject saleLabel;

        [SerializeField] Text saleLabelText;

        public Text Header => header;

        public Text Count => count;

        public Text UpgradeLevel => proficiencyLevel;

        public GameObject PriceGameObject => priceGameObject;

        public GameObject XPriceGameObject => xPriceGameObject;

        public GameObject UpgradeGameObject => upgradeGameObject;

        public ProgressBar ProgressBar => progressBar;

        public GameObject Arrow => arrow;

        public bool RareTextVisibility {
            set => rareText.gameObject.SetActive(value);
        }

        public string RareText {
            set => rareText.text = value;
        }

        public bool SaleLabelVisible {
            set => saleLabel.SetActive(value);
        }

        public string SaleLabelText {
            set => saleLabelText.text = value;
        }

        public void SetUpgradeColor(Color color) {
            unlockGraphic.color = color;
        }

        void Unlock() {
            GetComponent<Animator>().SetTrigger("Unlock");
        }

        public void AddPreview(string spriteUid, long count) {
            AddPreview(spriteUid).Count = count;
        }

        public GarageListItemContentPreviewComponent AddPreview(string spriteUid) {
            GameObject gameObject = Instantiate(previewPrefab);
            gameObject.transform.SetParent(previewContainer.transform, false);
            GarageListItemContentPreviewComponent component = gameObject.GetComponent<GarageListItemContentPreviewComponent>();
            component.SetImage(spriteUid);
            return component;
        }
    }
}