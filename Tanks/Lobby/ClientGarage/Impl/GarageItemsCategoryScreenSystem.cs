using System.Collections.Generic;
using System.Linq;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageItemsCategoryScreenSystem : ECSSystem {
        [OnEventFire]
        public void Localize(NodeAddedEvent e, SingleNode<GarageItemsCategoryLocalizedStringsComponent> screen,
            WeaponsButtonNode weponsButton, HullsButtonNode hullsButton, PaintButtonNode paintButton,
            GraffitiButtonNode graffitiButton, SuppliesButtonNode suppliesButton) {
            weponsButton.textMapping.Text = screen.component.WeaponsButtonLabel;
            hullsButton.textMapping.Text = screen.component.HullsButtonLabel;
            paintButton.textMapping.Text = screen.component.ColorsButtonLabel;
            suppliesButton.textMapping.Text = screen.component.SuppliesButtonLabel;
            graffitiButton.textMapping.Text = screen.component.GraffitiButtonLabel;
        }

        [OnEventFire]
        public void ShowShellItemsScreen(ButtonClickEvent e, SingleNode<ShellsButtonComponent> shellsButton,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem) =>
            ScheduleEvent<ShowItemsListScreenBySelectedItemEvent>(selectedItem.component.SelectedItem);

        [OnEventFire]
        public void ShowSkinItemsScreen(ButtonClickEvent e, SingleNode<SkinsButtonComponent> skinsButton,
            [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem) =>
            ScheduleEvent<ShowSkinItemsListScreenBySelectedItemEvent>(selectedItem.component.SelectedItem);

        [OnEventFire]
        public void ShowShellItemsScreen(ShowItemsListScreenBySelectedItemEvent e, SingleNode<WeaponItemComponent> item,
            [JoinByParentGroup] ICollection<SingleNode<ShellItemComponent>> shells) =>
            ShowItemsListScreen<CommonGarageItemsScreenComponent>(shells.Select(x => x.Entity));

        [OnEventFire]
        public void ShowSkinItemsScreen(ShowSkinItemsListScreenBySelectedItemEvent e, SingleNode<UserItemComponent> item,
            [JoinByParentGroup] ICollection<SingleNode<SkinItemComponent>> skins) =>
            ShowItemsListScreen<CommonGarageItemsScreenComponent>(skins.Select(x => x.Entity));

        [OnEventFire]
        public void ShowPaintItemsScreen(ButtonClickEvent e, SingleNode<PaintButtonComponent> colorsButton,
            [JoinAll] ICollection<SingleNode<PaintItemComponent>> paintItems) =>
            ShowItemsListScreen<CommonGarageItemsScreenComponent>(paintItems.Select(x => x.Entity));

        [OnEventFire]
        public void ShowGraffitiItemsScreen(ButtonClickEvent e, SingleNode<GraffitiButtonComponent> graffitiButton,
            [JoinAll] ICollection<SingleNode<GraffitiItemComponent>> graffitiItems) =>
            ShowItemsListScreen<GraffitiGarageItemsScreenComponent>(graffitiItems.Select(x => x.Entity));

        [OnEventFire]
        public void ShowWeaponItemsScreen(ButtonClickEvent e, SingleNode<WeaponsButtonComponent> weaponsButton,
            [JoinAll] ICollection<SingleNode<WeaponItemComponent>> weaponItems) =>
            ShowItemsListScreen<WeaponGarageItemsScreenComponent>(weaponItems.Select(x => x.Entity));

        [OnEventFire]
        public void ShowTankItemsScreen(ButtonClickEvent e, SingleNode<HullsButtonComponent> tanksButton,
            [JoinAll] ICollection<SingleNode<TankItemComponent>> tankItems) =>
            ShowItemsListScreen<HullGarageItemsScreenComponent>(tankItems.Select(x => x.Entity));

        [OnEventFire]
        public void ShowSupplyItemsScreen(ButtonClickEvent e, SingleNode<SuppliesButtonComponent> suppliesButton,
            [JoinAll] ICollection<SingleNode<SupplyCountComponent>> supplyItems) =>
            ShowItemsListScreen<GarageSuppliesScreenComponent>(supplyItems.Select(x => x.Entity));

        void ShowItemsListScreen<T>(IEnumerable<Entity> items) where T : MonoBehaviour, Component {
            Entity entity = CreateEntity("GarageItems");
            entity.AddComponent(new ItemsListForViewComponent(new List<Entity>(items)));
            entity.AddComponent<SelectedItemComponent>();
            ShowScreenLeftEvent<T> showScreenLeftEvent = new();
            showScreenLeftEvent.SetContext(entity, true);
            ScheduleEvent(showScreenLeftEvent, entity);
        }

        public class WeaponsButtonNode : Node {
            public TextMappingComponent textMapping;
            public WeaponsButtonComponent weaponsButton;
        }

        public class HullsButtonNode : Node {
            public HullsButtonComponent hullsButton;

            public TextMappingComponent textMapping;
        }

        public class PaintButtonNode : Node {
            public PaintButtonComponent paintButton;

            public TextMappingComponent textMapping;
        }

        public class GraffitiButtonNode : Node {
            public GraffitiButtonComponent graffitiButton;

            public TextMappingComponent textMapping;
        }

        public class SuppliesButtonNode : Node {
            public SuppliesButtonComponent suppliesButton;

            public TextMappingComponent textMapping;
        }

        public class ShowSkinItemsListScreenBySelectedItemEvent : Event { }
    }
}