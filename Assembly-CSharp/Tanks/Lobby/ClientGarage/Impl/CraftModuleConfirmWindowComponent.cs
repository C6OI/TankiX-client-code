using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CraftModuleConfirmWindowComponent : ConfirmWindowComponent {
        [SerializeField] protected TextMeshProUGUI additionalText;

        [SerializeField] LocalizedField module;

        [SerializeField] LocalizedField craftFor;

        [SerializeField] LocalizedField decline;

        [SerializeField] LocalizedField upgradeFor;

        [SerializeField] LocalizedField buyBlueprints;

        [SerializeField] Color greenColor;

        [SerializeField] Color whiteColor;

        [SerializeField] Image highlight;

        [SerializeField] Image fill;

        [SerializeField] protected ImageSkin icon;

        public string SpriteUid {
            set => icon.SpriteUid = value;
        }

        public GameObject CardPriceLabel { get; set; }

        public void Setup(string moduleName, string desc, string spriteUid, double price, bool craft, string currencySpriteId = "8", bool dontenoughtcard = false) {
            HeaderText = module.Value + " " + moduleName;
            additionalText.gameObject.SetActive(craft && dontenoughtcard);

            if (craft) {
                if (dontenoughtcard) {
                    ConfirmText = buyBlueprints.Value;
                } else {
                    ConfirmText = craftFor.Value;
                }
            } else {
                ConfirmText = price + "<sprite=" + currencySpriteId + ">";
            }

            DeclineText = decline.Value;
            MainText = desc;
            SpriteUid = spriteUid;
        }
    }
}