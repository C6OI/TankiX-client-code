using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class NewItemNotificationUnityComponent : BehaviourComponent {
        public Slider upgradeSlider;

        public AnimatedValueComponent upgradeAnimator;

        public int count;

        [SerializeField] TextMeshProUGUI headerElement;

        [SerializeField] GameObject containerContent;

        [SerializeField] TextMeshProUGUI itemNameElement;

        [SerializeField] ImageSkin itemIconSkin;

        [SerializeField] ImageSkin resourceIconSkin;

        [SerializeField] GameObject itemContent;

        [SerializeField] GameObject resourceContent;

        [SerializeField] Image borderImg;

        [SerializeField] TextMeshProUGUI rarityNameElement;

        [SerializeField] GameObject rareEffect;

        [SerializeField] GameObject epicEffect;

        [SerializeField] GameObject legendaryEffect;

        [SerializeField] LocalizedField commonText;

        [SerializeField] LocalizedField rareText;

        [SerializeField] LocalizedField epicText;

        [SerializeField] LocalizedField legendaryText;

        [SerializeField] public Material[] cardMaterial;

        public OutlineObject outline;

        public ModuleCardView view;

        [SerializeField] GameObject cardElement;

        public TextMeshProUGUI HeaderElement => headerElement;

        public TextMeshProUGUI ItemNameElement => itemNameElement;

        public bool ContainerContent {
            set => containerContent.SetActive(value);
        }

        public void SetItemImage(string spriteUid) {
            itemIconSkin.SpriteUid = spriteUid;
            itemContent.SetActive(true);
        }

        public void SetItemIcon(string spriteUid) {
            resourceIconSkin.SpriteUid = spriteUid;
            resourceContent.SetActive(true);
        }

        public void SetCardElement(int tier) {
            GetComponentInParent<LayoutElement>().preferredWidth = 300f;
            cardElement.SetActive(true);
            containerContent.SetActive(false);
            Renderer component = cardElement.GetComponent<Renderer>();
            component.sharedMaterial = cardMaterial[tier - 1];
        }

        public void SetItemRarity(GarageItem item) {
            Color rarityColor = item.Rarity.GetRarityColor();
            itemNameElement.color = rarityColor;
            borderImg.color = rarityColor;
            rarityNameElement.color = new Color(rarityColor.r, rarityColor.g, rarityColor.b, 0.3f);

            if (item.IsVisualItem) {
                switch (item.Rarity) {
                    case ItemRarityType.COMMON:
                        rarityNameElement.text = string.Format("[{0}]", commonText.Value);
                        break;

                    case ItemRarityType.RARE:
                        rarityNameElement.text = string.Format("[{0}]", rareText.Value);
                        rareEffect.SetActive(true);
                        break;

                    case ItemRarityType.EPIC:
                        rarityNameElement.text = string.Format("[{0}]", epicText.Value);
                        epicEffect.SetActive(true);
                        break;

                    case ItemRarityType.LEGENDARY:
                        rarityNameElement.text = string.Format("[{0}]", legendaryText.Value);
                        legendaryEffect.SetActive(true);
                        break;
                }
            } else {
                rarityNameElement.gameObject.SetActive(false);
            }
        }
    }
}