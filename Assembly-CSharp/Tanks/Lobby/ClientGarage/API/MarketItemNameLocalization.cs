using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLocale.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class MarketItemNameLocalization : MonoBehaviour {
        public static MarketItemNameLocalization Instance;

        [SerializeField] LocalizedField skin;

        [SerializeField] LocalizedField coating;

        [SerializeField] LocalizedField paint;

        [SerializeField] LocalizedField container;

        [SerializeField] LocalizedField graffity;

        [SerializeField] LocalizedField shell;

        [SerializeField] LocalizedField avatar;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        void Awake() {
            Instance = this;
        }

        public static string GetDetailedName(Entity marketItem) {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(marketItem);
            string savedLocaleCode = LocaleUtils.GetSavedLocaleCode();

            if ("ru".Equals(savedLocaleCode)) {
                return Instance.GetCategoryName(marketItem) + " " + Instance.GetGarageItemName(item);
            }

            return Instance.GetGarageItemName(item) + " " + Instance.GetCategoryName(marketItem);
        }

        public string GetCategoryName(Entity entity) {
            if (entity == null) {
                return string.Empty;
            }

            if (entity.HasComponent<SkinItemComponent>()) {
                return skin.Value;
            }

            if (entity.HasComponent<TankPaintItemComponent>()) {
                return paint.Value;
            }

            if (entity.HasComponent<WeaponPaintItemComponent>()) {
                return coating.Value;
            }

            if (entity.HasComponent<ContainerMarkerComponent>()) {
                return container.Value;
            }

            if (entity.HasComponent<GraffitiItemComponent>()) {
                return graffity.Value;
            }

            if (entity.HasComponent<ShellItemComponent>()) {
                return shell.Value;
            }

            if (entity.HasComponent<AvatarItemComponent>()) {
                return avatar.Value;
            }

            return string.Empty;
        }

        public string GetGarageItemName(GarageItem item) {
            VisualItem visualItem = item as VisualItem;

            if (visualItem != null && visualItem.ParentItem != null && !visualItem.Name.Contains(visualItem.ParentItem.Name)) {
                return string.Format("{0} {1}", visualItem.ParentItem.Name, visualItem.Name);
            }

            return item.Name;
        }

        public static string GetFullItemDescription(GarageItem item, bool withParentItemName, string commonRarity = "", string rareRarity = "", string epicRarity = "",
            string legendaryRarity = "") {
            string text = !withParentItemName ? Instance.GetCategoryName(item.MarketItem) + " " + item.Name : GetDetailedName(item.MarketItem);
            string text2 = string.Empty;
            ItemRarityType rarity = item.Rarity;

            switch (item.Rarity) {
                case ItemRarityType.COMMON:
                    text2 += commonRarity;
                    break;

                case ItemRarityType.RARE:
                    text2 += rareRarity;
                    break;

                case ItemRarityType.EPIC:
                    text2 += epicRarity;
                    break;

                case ItemRarityType.LEGENDARY:
                    text2 += legendaryRarity;
                    break;
            }

            if (!string.IsNullOrEmpty(text2)) {
                string text3 = text;
                text = text3 + "\n<color=#" + rarity.GetRarityColor().ToHexString() + ">" + text2 + "</color>";
            }

            string description = item.Description;

            if (!string.IsNullOrEmpty(description)) {
                text = text + "\n" + description;
            }

            return text;
        }

        public static string GetFullItemDescription(Entity marketItem, string commonRarity = "", string rareRarity = "", string epicRarity = "", string legendaryRarity = "") {
            string text = GetDetailedName(marketItem);
            string text2 = string.Empty;
            ItemRarityType rarityType = marketItem.GetComponent<ItemRarityComponent>().RarityType;

            switch (rarityType) {
                case ItemRarityType.COMMON:
                    text2 += commonRarity;
                    break;

                case ItemRarityType.RARE:
                    text2 += rareRarity;
                    break;

                case ItemRarityType.EPIC:
                    text2 += epicRarity;
                    break;

                case ItemRarityType.LEGENDARY:
                    text2 += legendaryRarity;
                    break;
            }

            if (!string.IsNullOrEmpty(text2)) {
                string text3 = text;
                text = text3 + "\n<color=#" + rarityType.GetRarityColor().ToHexString() + ">" + text2 + "</color>";
            }

            string description = marketItem.GetComponent<DescriptionItemComponent>().Description;

            if (!string.IsNullOrEmpty(description)) {
                text = text + "\n" + description;
            }

            return text;
        }
    }
}