using System.Collections.Generic;
using System.IO;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class ItemPreviewSystem : ECSSystem {
        [OnEventFire]
        public void AddPreviewGroup(NodeAddedEvent e, SingleNode<GarageItemComponent> garageItem) {
            DirectoryInfo parent =
                Directory.GetParent(((EntityInternal)garageItem.Entity).TemplateAccessor.Get().ConfigPath);

            garageItem.Entity.AddComponent(
                new PreviewGroupComponent(ConfigurationEntityIdCalculator.Calculate(parent.ToString())));
        }

        [OnEventFire]
        public void AddMarketDefaultSkin(NodeAddedEvent e, DefaultSkinItemNode defaultSkin) =>
            defaultSkin.Entity.AddComponent<HangarItemPreviewComponent>();

        [OnEventFire]
        public void PreviewMounted(NodeAddedEvent e, MountedUserItemNode mountedItem) => PreviewItem(mountedItem.Entity);

        [OnEventFire]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<GarageItemComponent> item,
            [JoinAll] ActiveGarageScreenNode screen) {
            if (screen.Entity.HasComponent<GarageItemsScreenComponent>() ||
                screen.Entity.HasComponent<GarageSuppliesScreenComponent>()) {
                PreviewItem(item.Entity);
            }
        }

        [OnEventFire]
        public void ItemSelected(NodeAddedEvent e, ActiveItemPropertiesScreenNode itemPropertiesScreen,
            [JoinByScreen] WeaponItemWithPropertiesNotPreviewNode weapon) => PreviewItem(weapon.Entity);

        [OnEventFire]
        public void ItemSelected(NodeAddedEvent e, ActiveItemPropertiesScreenNode itemPropertiesScreen,
            [JoinByScreen] HullItemWithPropertiesNotPreviewNode hull) => PreviewItem(hull.Entity);

        [OnEventFire]
        public void ShellSelected(ListItemSelectedEvent e, SingleNode<ShellItemComponent> shell,
            [JoinByParentGroup] WeaponNotPreviewNode weapon) => PreviewItem(weapon.Entity);

        [OnEventFire]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<SkinItemComponent> skin,
            [JoinByParentGroup] HullNotPreviewNode hull) => PreviewItem(hull.Entity);

        [OnEventFire]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<SkinItemComponent> skin,
            [JoinByParentGroup] WeaponNotPreviewNode weapon) => PreviewItem(weapon.Entity);

        [OnEventFire]
        public void RevertToMountedForSkins(NodeRemoveEvent e, SingleNode<GarageItemsScreenComponent> itemsScreen,
            [JoinAll] [Combine] MountedUserSkinNode mountedItem) => PreviewItem(mountedItem.Entity);

        [OnEventFire]
        public void RevertToMounted(NodeAddedEvent e, SingleNode<ScreenComponent> screen,
            [JoinAll] [Combine] MountedUserItemNode mountedItem) {
            if (screen.component.gameObject.GetComponent<ItemsListScreenComponent>() == null) {
                PreviewItem(mountedItem.Entity);
            }
        }

        void PreviewItem(Entity item) => ScheduleEvent<PrewievEvent>(item);

        [OnEventFire]
        public void ApplyPreview(PrewievEvent e, GraffitiNotPreviewNode graffiti,
            [JoinAll] Optional<GraffitiPreviewNode> otherGraffiti) {
            if (otherGraffiti.IsPresent()) {
                otherGraffiti.Get().Entity.RemoveComponent<HangarItemPreviewComponent>();
            }

            graffiti.Entity.AddComponent<HangarItemPreviewComponent>();
        }

        [OnEventFire]
        public void ApplyPreview(PrewievEvent e, NotGraffitiNode node) => ApplyPreview(node.Entity);

        public void ApplyPreview(Entity item) {
            IList<PreviewNode> list = Select<PreviewNode>(item, typeof(PreviewGroupComponent));

            list.ForEach(delegate(PreviewNode p) {
                if (p.Entity != item) {
                    p.Entity.RemoveComponent<HangarItemPreviewComponent>();
                }
            });

            if (!item.HasComponent<HangarItemPreviewComponent>()) {
                item.AddComponent<HangarItemPreviewComponent>();
            }
        }

        public class PrewievEvent : Event { }

        public class PreviewNode : Node {
            public GarageItemComponent garageItem;

            public HangarItemPreviewComponent hangarItemPreview;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class WeaponNotPreviewNode : Node {
            public UserItemComponent userItem;
            public WeaponItemComponent weaponItem;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class HullNotPreviewNode : Node {
            public TankItemComponent tankItem;

            public UserItemComponent userItem;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class WeaponItemWithPropertiesNotPreviewNode : Node {
            public WeaponItemComponent weaponItem;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class HullItemWithPropertiesNotPreviewNode : Node {
            public TankItemComponent tankItem;
        }

        public class UserItemNode : Node {
            public GarageItemComponent garageItem;
            public UserGroupComponent userGroup;

            public UserItemComponent userItem;
        }

        public class MountedUserItemNode : UserItemNode {
            public MountedItemComponent mountedItem;
        }

        public class MountedUserSkinNode : MountedUserItemNode {
            public SkinItemComponent skinItem;
        }

        public class DefaultSkinItemNode : Node {
            public DefaultSkinItemComponent defaultSkinItem;
            public MarketItemComponent marketItem;

            public ParentGroupComponent parentGroup;
        }

        public class ActiveGarageScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        public class ActiveItemPropertiesScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ItemPropertiesScreenComponent itemPropertiesScreen;
        }

        [Not(typeof(GraffitiItemComponent))]
        public class NotGraffitiNode : Node {
            public GarageItemComponent garageItem;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class GraffitiNotPreviewNode : Node {
            public GraffitiItemComponent graffitiItem;
        }

        public class GraffitiPreviewNode : PreviewNode {
            public GraffitiItemComponent graffitiItem;
        }
    }
}