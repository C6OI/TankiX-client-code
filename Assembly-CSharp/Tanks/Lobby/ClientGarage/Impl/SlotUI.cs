using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SlotUI : MonoBehaviour {
        [SerializeField] ImageSkin moduleIcon;

        [SerializeField] PaletteColorField exceptionalColor;

        [SerializeField] PaletteColorField epicColor;

        [SerializeField] Image lockImage;

        int unlockLevel;

        public string Icon {
            set => moduleIcon.SpriteUid = value;
        }

        public bool Locked {
            set {
                lockImage.gameObject.SetActive(value);
                moduleIcon.gameObject.SetActive(!value);
            }
        }

        public void SetEpic() {
            moduleIcon.GetComponent<Image>().color = epicColor;
        }

        public void SetLegendary() {
            moduleIcon.GetComponent<Image>().color = exceptionalColor;
        }

        public void SetNormal() {
            moduleIcon.GetComponent<Image>().color = Color.white;
        }

        public void SetLockedTooltip(int unlockLevel) {
            TooltipShowBehaviour component = GetComponent<TooltipShowBehaviour>();

            if (component != null) {
                component.showTooltip = true;
                component.TipText = component.localizedTip.Value.Replace("{0}", unlockLevel.ToString());
            }
        }

        public void SetUnlockedTooltip(string name) {
            TooltipShowBehaviour component = GetComponent<TooltipShowBehaviour>();

            if (component != null) {
                if (string.IsNullOrEmpty(name)) {
                    component.showTooltip = false;
                    return;
                }

                component.showTooltip = true;
                component.TipText = name;
            }
        }
    }
}