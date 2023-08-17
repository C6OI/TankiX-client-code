using System.Collections.Generic;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplayShellsButtonSystem : ECSSystem {
        [OnEventFire]
        public void SetShelsButtonText(NodeAddedEvent evt, ShellsButtonTextNode shellsButton) {
            Text text = shellsButton.shellsButton.gameObject.GetComponentsInChildren<Text>(true)[0];
            text.text = shellsButton.shellsButtonText.ShellsText;
        }

        [OnEventFire]
        public void UpdateShellsButton(ListItemSelectedEvent e, Node any, [JoinAll] WeaponGarageItemsScreenNode screen,
            [Mandatory] [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem) =>
            ScheduleEvent<UpdateShellsButtonEvent>(selectedItem.component.SelectedItem);

        [OnEventFire]
        public void EnableShellButton(NodeAddedEvent e, WeaponUserItemNode item,
            [JoinByParentGroup] ICollection<ShellMarketItemNode> shellItems, WeaponUserItemNode item1,
            [JoinAll] WeaponGarageItemsScreenNode screen) =>
            screen.weaponGarageItemsScreen.ShellsButton.gameObject.SetActive(shellItems.Count > 1);

        [OnEventFire]
        public void EnableShellButton(UpdateShellsButtonEvent e, SingleNode<UserItemComponent> item,
            [JoinByParentGroup] ICollection<ShellMarketItemNode> shellItems, SingleNode<UserItemComponent> item1,
            [JoinAll] [Mandatory] WeaponGarageItemsScreenNode screen) =>
            screen.weaponGarageItemsScreen.ShellsButton.gameObject.SetActive(shellItems.Count > 1);

        [OnEventFire]
        public void DisableShellButton(UpdateShellsButtonEvent e, SingleNode<MarketItemComponent> item,
            [Mandatory] [JoinAll] WeaponGarageItemsScreenNode screen) =>
            screen.weaponGarageItemsScreen.ShellsButton.gameObject.SetActive(false);

        public class ShellsButtonTextNode : Node {
            public ShellsButtonComponent shellsButton;
            public ShellsButtonTextComponent shellsButtonText;
        }

        public class WeaponGarageItemsScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public GarageItemsScreenComponent garageItemsScreen;

            public WeaponGarageItemsScreenComponent weaponGarageItemsScreen;
        }

        public class WeaponUserItemNode : Node {
            public ParentGroupComponent parentGroup;

            public UserItemComponent userItem;
            public WeaponItemComponent weaponItem;
        }

        public class ShellMarketItemNode : Node {
            public MarketItemComponent marketItem;
            public ShellItemComponent shellItem;
        }
    }
}