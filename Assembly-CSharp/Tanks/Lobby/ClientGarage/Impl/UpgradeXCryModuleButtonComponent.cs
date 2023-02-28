using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradeXCryModuleButtonComponent : UpgradeModuleBaseButtonComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        [SerializeField] LocalizedField buyBlueprints;

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
            if (moduleLevel >= 0 && maxCardCount != 0) {
                gameObject.SetActive(true);
                Activate();

                if (cardsCount < maxCardCount) {
                    titleText.text = buyBlueprints.Value;
                    Color white = Color.white;
                    white.a = 0f;
                    fill.color = white;
                    Image image = border;
                    Color color = notEnoughColor;
                    titleText.color = color;
                    image.color = color;
                } else {
                    bool flag = userXCryCount >= priceXCry;

                    titleText.text = moduleLevel <= -1 ? activate : upgrade.Value + "  " +
                                                                    (flag ? priceXCry.ToString() : "<color=#" + notEnoughTextColor.ToHexString() + ">" + priceXCry + "</color>") +
                                                                    "<sprite=9>";

                    Image image2 = border;
                    Color color = !flag ? notEnoughColor : enoughColor;
                    titleText.color = color;
                    color = color;
                    fill.color = color;
                    image2.color = color;
                }
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}