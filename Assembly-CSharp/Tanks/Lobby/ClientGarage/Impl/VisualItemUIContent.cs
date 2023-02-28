using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class VisualItemUIContent : MonoBehaviour, ListItemContent {
        static readonly int EQUIPPED_STATE = Animator.StringToHash("Equipped");

        static readonly int INSTANT_STATE = Animator.StringToHash("Instant");

        [SerializeField] new TextMeshProUGUI name;

        [SerializeField] ImageSkin preview;

        [SerializeField] ListItemPrices prices;

        [SerializeField] TextMeshProUGUI containerLabel;

        [SerializeField] TextMeshProUGUI upgradesLabel;

        [SerializeField] LocalizedField upgradesRequiredText;

        [SerializeField] LocalizedField _commonString;

        [SerializeField] LocalizedField _rareString;

        [SerializeField] LocalizedField _epicString;

        [SerializeField] LocalizedField _legendaryString;

        VisualItem item;

        public void SetDataProvider(object dataProvider) {
            VisualItem visualItem = (VisualItem)dataProvider;

            if (visualItem.WaitForBuy && visualItem.UserItem != null) {
                visualItem.WaitForBuy = false;
                SetNameTo(name, visualItem);
                UpdatePrice(GetItemLevel(visualItem));
                UpdateState(false);
            } else if (visualItem != item) {
                item = visualItem;
                SetNameTo(name, visualItem);
                UpdatePrice(GetItemLevel(visualItem));
                UpdateState(true);
                preview.SpriteUid = visualItem.Preview;
                RectTransform component = preview.GetComponent<RectTransform>();

                if (visualItem.Type == VisualItem.VisualItemType.Paint || visualItem.Type == VisualItem.VisualItemType.Coating) {
                    component.anchoredPosition = new Vector2(-76f, -88f);
                    component.sizeDelta = new Vector2(1121f, 544f);
                } else {
                    component.anchoredPosition = Vector2.zero;
                    component.sizeDelta = new Vector2(500f, 300f);
                }
            } else {
                UpdatePrice(GetItemLevel(visualItem));
                UpdateState(false);
            }
        }

        public void Select() {
            if (!item.IsSelected) {
                this.SendEvent<ListItemSelectedEvent>(item.UserItem ?? item.MarketItem);
            }
        }

        void SendChanged() {
            if (item.IsSelected) {
                SendMessageUpwards("OnItemChanged", item);
            }
        }

        int GetItemLevel(VisualItem visualItem) {
            if (visualItem.ParentItem != null) {
                return visualItem.ParentItem.UpgradeLevel;
            }

            return 0;
        }

        void SetNameTo(TextMeshProUGUI tmpName, VisualItem newItem) {
            tmpName.text = newItem.Name;
            tmpName.color = newItem.Rarity.GetRarityColor();
            TooltipShowBehaviour componentInParent = GetComponentInParent<TooltipShowBehaviour>();

            if (!(componentInParent == null)) {
                componentInParent.TipText = MarketItemNameLocalization.GetFullItemDescription(newItem, false, _commonString, _rareString, _epicString, _legendaryString);
            }
        }

        void UpdatePrice(int currentLevel) {
            prices.Set(item);
            upgradesLabel.text = string.Empty;

            if (!prices.gameObject.activeSelf) {
                if (item.UserItem == null) {
                    upgradesLabel.gameObject.SetActive(false);
                    containerLabel.gameObject.SetActive(item.IsContainerItem);

                    if (!item.IsContainerItem) {
                        int restrictionLevel = item.RestrictionLevel;

                        if (item.IsRestricted) {
                            upgradesLabel.gameObject.SetActive(restrictionLevel > currentLevel);
                            upgradesLabel.text = string.Format(upgradesRequiredText.Value, restrictionLevel);
                        }
                    }
                } else {
                    containerLabel.gameObject.SetActive(false);
                    int restrictionLevel2 = item.RestrictionLevel;

                    if (item.IsRestricted) {
                        upgradesLabel.gameObject.SetActive(restrictionLevel2 > currentLevel);
                        upgradesLabel.text = string.Format(upgradesRequiredText.Value, restrictionLevel2);
                    }
                }

                if (item.Type == VisualItem.VisualItemType.Graffiti && item.ParentItem != null && item.IsRestricted) {
                    int restrictionLevel3 = item.RestrictionLevel;
                    upgradesLabel.text = string.Format(upgradesRequiredText.Value, restrictionLevel3);
                    upgradesLabel.text += string.Format(" ({0})", item.ParentItem.Name);
                }
            } else {
                containerLabel.gameObject.SetActive(false);
                upgradesLabel.gameObject.SetActive(false);
            }
        }

        void UpdateState(bool instant) {
            Animator component = GetComponent<Animator>();
            bool @bool = component.GetBool(EQUIPPED_STATE);

            if (item.UserItem != null) {
                if (item.UserItem.HasComponent<MountedItemComponent>() && !@bool) {
                    component.SetBool(INSTANT_STATE, instant);
                    component.SetBool(EQUIPPED_STATE, true);
                    SendChanged();
                } else if (!item.UserItem.HasComponent<MountedItemComponent>() && @bool) {
                    component.SetBool(INSTANT_STATE, instant);
                    component.SetBool(EQUIPPED_STATE, false);
                    SendChanged();
                }
            } else if (@bool) {
                component.SetBool(INSTANT_STATE, instant);
                component.SetBool(EQUIPPED_STATE, false);
            }
        }
    }
}