using Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageListItemContentComponent : MonoBehaviour, Component {
        [SerializeField] Text header;

        [SerializeField] Text count;

        [SerializeField] Text proficiencyLevel;

        [SerializeField] ProgressBar progressBar;

        [SerializeField] GameObject priceGameObject;

        [SerializeField] GameObject xPriceGameObject;

        [SerializeField] GameObject upgradeGameObject;

        [SerializeField] ImageSkin skin;

        [SerializeField] Image image;

        [SerializeField] GameObject arrow;

        [SerializeField] Graphic unlockGraphic;

        public Text Header => header;

        public Text Count => count;

        public Text ProficiencyLevel => proficiencyLevel;

        public GameObject PriceGameObject => priceGameObject;

        public GameObject XPriceGameObject => xPriceGameObject;

        public GameObject UpgradeGameObject => upgradeGameObject;

        public ProgressBar ProgressBar => progressBar;

        public GameObject Arrow => arrow;

        public void SetImage(string spriteUid) {
            skin.SpriteUid = spriteUid;
            image.enabled = true;
            skin.enabled = true;
        }

        public void SetEmptyImage() {
            skin.ResetSkin();
            image.enabled = false;
            skin.enabled = false;
        }

        public void SetUpgradeColor(Color color) => unlockGraphic.color = color;

        void Unlock() => GetComponent<Animator>().SetTrigger("Unlock");
    }
}