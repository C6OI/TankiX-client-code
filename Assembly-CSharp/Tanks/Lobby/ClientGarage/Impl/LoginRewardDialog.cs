using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LoginRewardDialog : ConfirmDialogComponent {
        public RectTransform itemsContainer;

        public ReleaseGiftItemComponent itemPrefab;

        public float itemsShowDelay = 0.6f;

        public ImageSkin leagueIcon;

        public TextMeshProUGUI headerText;

        public TextMeshProUGUI text;

        public LoginRewardAllItemsContainer allItems;

        [SerializeField] LocalizedField paint;

        [SerializeField] LocalizedField coating;

        [SerializeField] LocalizedField dayShort;

        [SerializeField] LocalizedField container;

        [SerializeField] LocalizedField premium;

        public List<Entity> marketItems = new();

        public void ScrollToCurrentDay() {
            allItems.ScrollToCurrentDay();
        }

        public string GetRewardItemName(Entity marketItemEntity) {
            string text = marketItemEntity.GetComponent<DescriptionItemComponent>().Name;

            if (marketItemEntity.HasComponent<WeaponPaintItemComponent>()) {
                text = coating.Value + "\n" + text;
            } else if (marketItemEntity.HasComponent<PaintItemComponent>()) {
                text = paint.Value + "\n" + text;
            } else if (marketItemEntity.HasComponent<ContainerMarkerComponent>()) {
                text = container.Value + "\n" + text;
            } else if (marketItemEntity.HasComponent<PremiumBoostItemComponent>()) {
                text = premium.Value + " {0} " + dayShort.Value;
            }

            return text;
        }

        public string GetRewardItemNameWithAmount(Entity marketItemEntity, int amount) {
            string text = marketItemEntity.GetComponent<DescriptionItemComponent>().Name;

            if (marketItemEntity.HasComponent<WeaponPaintItemComponent>()) {
                return coating.Value + " " + text;
            }

            if (marketItemEntity.HasComponent<PaintItemComponent>()) {
                return paint.Value + " " + text;
            }

            if (marketItemEntity.HasComponent<PremiumBoostItemComponent>()) {
                return premium.Value + " " + amount + " " + dayShort.Value;
            }

            if (marketItemEntity.HasComponent<ContainerMarkerComponent>()) {
                return container.Value + "\n" + text + " x" + amount;
            }

            return text + " x" + amount;
        }
    }
}