using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ContainerContentItemUIContent : MonoBehaviour, ListItemContent {
        [SerializeField] TextMeshProUGUI _name;

        [SerializeField] GameObject _own;

        [SerializeField] ImageSkin _preview;

        [SerializeField] LocalizedField _commonString;

        [SerializeField] LocalizedField _rareString;

        [SerializeField] LocalizedField _epicString;

        [SerializeField] LocalizedField _legendaryString;

        GarageItem _item;

        public void SetDataProvider(object dataProvider) {
            if (_item == dataProvider) {
                return;
            }

            _item = dataProvider as GarageItem;

            if (_item == null) {
                return;
            }

            SetNameTo(_name, _item);
            _own.SetActive(_item.UserItem != null);
            _preview.SpriteUid = _item.Preview;
            RectTransform component = _preview.GetComponent<RectTransform>();

            if (dataProvider is PremiumItem) {
                component.anchoredPosition = Vector2.zero;
                _preview.GetComponent<Image>().SetNativeSize();
                return;
            }

            VisualItem visualItem = dataProvider as VisualItem;

            if (visualItem != null) {
                if (visualItem.Type == VisualItem.VisualItemType.Paint || visualItem.Type == VisualItem.VisualItemType.Coating) {
                    component.anchoredPosition = new Vector2(-76f, -88f);
                    component.sizeDelta = new Vector2(1121f, 544f);
                } else {
                    component.anchoredPosition = Vector2.zero;
                    component.sizeDelta = new Vector2(500f, 300f);
                }
            }
        }

        public void Select() {
            VisualItem visualItem = _item as VisualItem;
            ContainerContentUI componentInParent = GetComponentInParent<ContainerContentUI>();
            ContainersUI componentInParent2 = GetComponentInParent<ContainersUI>();
            componentInParent.GraffitiRoot.SetActive(visualItem != null && visualItem.Type == VisualItem.VisualItemType.Graffiti && componentInParent2.previewMode);

            if (visualItem != null) {
                this.SendEvent(new ResetPreviewEvent {
                    ExceptPreviewGroup = _item.MarketItem.GetComponent<PreviewGroupComponent>().Key
                });
            }

            this.SendEvent<ListItemSelectedEvent>(_item.UserItem ?? _item.MarketItem);
        }

        void SetNameTo(TextMeshProUGUI tmpName, GarageItem newItem) {
            ContainerContentUI componentInParent = GetComponentInParent<ContainerContentUI>();
            string categoryName = MarketItemNameLocalization.Instance.GetCategoryName(newItem.MarketItem);
            string text = string.Empty;

            if (newItem.MarketItem != null) {
                text = componentInParent.Item.GetLocalizedContentItemName(newItem.MarketItem.Id);
            }

            if (string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(categoryName)) {
                text = string.Format("{0} {1}", categoryName, MarketItemNameLocalization.Instance.GetGarageItemName(_item));
            }

            tmpName.text = text;
            tmpName.color = newItem.Rarity.GetRarityColor();

            GetComponentInParent<TooltipShowBehaviour>().TipText =
                MarketItemNameLocalization.GetFullItemDescription(newItem, true, _commonString, _rareString, _epicString, _legendaryString);
        }

        public void UpdateOwn() {
            if (_item != null) {
                _own.SetActive(_item.UserItem != null);
            }
        }
    }
}