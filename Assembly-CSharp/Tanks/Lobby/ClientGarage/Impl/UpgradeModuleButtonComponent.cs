using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradeModuleButtonComponent : UpgradeModuleBaseButtonComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        [SerializeField] LocalizedField notEnoughBlueprints;

        [SerializeField] GameObject content;

        public void OnPointerEnter(PointerEventData eventData) {
            if (content != null) {
                ModulePropertyView[] componentsInChildren = content.GetComponentsInChildren<ModulePropertyView>();

                foreach (ModulePropertyView modulePropertyView in componentsInChildren) {
                    modulePropertyView.FillNext.SetActive(true);
                    modulePropertyView.NextString.SetActive(true);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (content != null) {
                ModulePropertyView[] componentsInChildren = content.GetComponentsInChildren<ModulePropertyView>();

                foreach (ModulePropertyView modulePropertyView in componentsInChildren) {
                    modulePropertyView.FillNext.SetActive(false);
                    modulePropertyView.NextString.SetActive(false);
                }
            }
        }

        public override void Setup(int moduleLevel, int cardsCount, int maxCardCount, int price, int priceXCry, int userCryCount, int userXCryCount) {
            if (moduleLevel >= 0) {
                gameObject.SetActive(true);

                if (maxCardCount == 0) {
                    FullUpgraded();
                    return;
                }

                Activate();
                Color color;

                if (cardsCount < maxCardCount) {
                    GetComponent<Button>().interactable = false;
                    titleText.text = notEnoughBlueprints.Value;
                    fill.color = notEnoughFillColor;
                    Image image = border;
                    color = notEnoughColor;
                    titleText.color = color;
                    image.color = color;
                    return;
                }

                GetComponent<Button>().interactable = true;
                bool flag = userCryCount >= price;

                titleText.text = moduleLevel <= -1 ? activate
                                     : upgrade.Value + "  " + (flag ? price.ToString() : "<color=#" + notEnoughTextColor.ToHexString() + ">" + price + "</color>") + "<sprite=8>";

                Image image2 = border;
                color = !flag ? notEnoughColor : enoughColor;
                titleText.color = color;
                color = color;
                fill.color = color;
                image2.color = color;
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}