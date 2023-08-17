using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplayMountButtonSystem : ECSSystem {
        [OnEventFire]
        public void HideMountButton(ListItemSelectedEvent e, BuyableMarketItemNode item, [JoinAll] ScreenNode screenNode) =>
            HideMountButton(screenNode);

        [OnEventFire]
        public void HideMountButton(ListItemSelectedEvent e, NotMountedNotSkinUserItemWithRestrictionNode item,
            [JoinAll] ScreenNode screenNode) => ShowMountButtonForRestrictedItem(screenNode);

        [OnEventFire]
        public void HideMountButton(ListItemSelectedEvent e, NotMountedSkinUserItemWithRestrictionNode item,
            [JoinAll] ScreenNode screenNode) => ShowMountButtonForSkinItem(screenNode, true);

        [OnEventFire]
        public void HideMountButton(NodeAddedEvent e, NotMountedUserItemWithRestrictionNode item,
            [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode) {
            if (selectedItemNode.component.SelectedItem == item.Entity) {
                HideMountButton(screenNode);
            }
        }

        [OnEventFire]
        public void ShowEquippedButton(ListItemSelectedEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode) =>
            ShowMountButton(screenNode, false);

        [OnEventFire]
        public void ShowMountButton(ListItemSelectedEvent e, NotMountedSkinUserItemNode item,
            [JoinAll] ScreenNode screenNode) => ShowMountButtonForSkinItem(screenNode, false);

        [OnEventFire]
        public void ShowMountButton(ListItemSelectedEvent e, NotMountedNotSkinUserItemNode item,
            [JoinAll] ScreenNode screenNode) => ShowMountButton(screenNode, true);

        [OnEventFire]
        public void ShowEquippedButton(NodeAddedEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode) {
            if (selectedItemNode.component.SelectedItem == item.Entity) {
                ShowMountButton(screenNode, false);
            }
        }

        [OnEventFire]
        public void ShowMountButton(NodeRemoveEvent e, NotMountedUserItemWithRestrictionNode item,
            [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode) {
            if (selectedItemNode.component.SelectedItem == item.Entity) {
                ShowMountButton(screenNode, true);
            }
        }

        [OnEventFire]
        public void ShowMountButton(NodeAddedEvent e, NotMountedUserItemNode item, [JoinAll] ScreenNode screenNode,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode) {
            if (selectedItemNode.component.SelectedItem == item.Entity) {
                ShowMountButton(screenNode, true);
            }
        }

        void ShowMountButtonForSkinItem(ScreenNode screenNode, bool hasRestriction) {
            if (hasRestriction) {
                ShowMountButtonForRestrictedItem(screenNode);
            } else {
                ShowMountButton(screenNode, true);
            }
        }

        void HideMountButton(ScreenNode screenNode) {
            screenNode.garageItemsScreen.MountLabel.gameObject.SetActive(false);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetActive(false);
        }

        void ShowMountButtonForRestrictedItem(ScreenNode screenNode) {
            screenNode.garageItemsScreen.MountLabel.gameObject.SetActive(false);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetInteractable(false);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetActive(true);
        }

        void ShowMountButton(ScreenNode screenNode, bool interactable) {
            screenNode.garageItemsScreen.MountLabel.gameObject.SetActive(!interactable);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetActive(interactable);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetInteractable(interactable);

            if (interactable) {
                EventSystem.current.SetSelectedGameObject(screenNode.garageItemsScreen.MountItemButton.gameObject);
            }
        }

        [OnEventFire]
        public void
            AddMountLabel(NodeAddedEvent e, SingleNode<MountLabelComponent> mountLabel, [JoinAll] ScreenNode screen) =>
            mountLabel.component.GetComponent<Text>().text = screen.garageItemsScreenText.MountedText;

        [OnEventFire]
        public void SetMountText(NodeAddedEvent e, MountItemButtonNode node) =>
            node.textMapping.Text = node.mountItemButtonText.MountText;

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenComponent garageItemsScreen;

            public GarageItemsScreenTextComponent garageItemsScreenText;

            public ScreenComponent screen;
        }

        public class UserItemNode : Node {
            public UserItemComponent userItem;
        }

        public class MountedUserItemNode : UserItemNode {
            public MountedItemComponent mountedItem;
        }

        public class WeaponUserItemNode : UserItemNode {
            public WeaponItemComponent weaponItem;
        }

        public class TankUserItemNode : UserItemNode {
            public TankItemComponent tankItem;
        }

        [Not(typeof(MountedItemComponent))]
        [Not(typeof(RestrictedByUpgradeLevelComponent))]
        public class NotMountedUserItemNode : UserItemNode { }

        [Not(typeof(SkinItemComponent))]
        public class NotMountedNotSkinUserItemNode : NotMountedUserItemNode { }

        public class NotMountedSkinUserItemNode : NotMountedUserItemNode {
            public SkinItemComponent skinItem;
        }

        public class BuyableMarketItemNode : Node {
            public MarketItemComponent marketItem;

            public PriceItemComponent priceItem;
        }

        [Not(typeof(MountedItemComponent))]
        public class NotMountedUserItemWithRestrictionNode : UserItemNode {
            public RestrictedByUpgradeLevelComponent restrictedByUpgradeLevel;
        }

        [Not(typeof(SkinItemComponent))]
        public class NotMountedNotSkinUserItemWithRestrictionNode : NotMountedUserItemWithRestrictionNode { }

        public class NotMountedSkinUserItemWithRestrictionNode : NotMountedUserItemWithRestrictionNode {
            public SkinItemComponent skinItem;
        }

        public class NotMountedGraffitiUserItemWithRestrictionNode : NotMountedUserItemWithRestrictionNode {
            public GraffitiItemComponent graffitiItem;
        }

        public class MountItemButtonNode : Node {
            public MountItemButtonComponent mountItemButton;

            public MountItemButtonTextComponent mountItemButtonText;

            public TextMappingComponent textMapping;
        }
    }
}