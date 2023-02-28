using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class ItemPreviewSystem : ItemPreviewBaseSystem {
        [OnEventFire]
        public void AddPreviewGroup(NodeAddedEvent e, SingleNode<GarageItemComponent> garageItem) {
            DirectoryInfo parent = Directory.GetParent(((EntityInternal)garageItem.Entity).TemplateAccessor.Get().ConfigPath);
            garageItem.Entity.AddComponent(new PreviewGroupComponent(ConfigurationEntityIdCalculator.Calculate(parent.ToString())));
        }

        [OnEventFire]
        public void AddMarketDefaultSkin(NodeAddedEvent e, DefaultSkinItemNode defaultSkin) {
            defaultSkin.Entity.AddComponent<HangarItemPreviewComponent>();
        }

        [OnEventFire]
        public void PreviewMounted(NodeAddedEvent e, [Combine] NotGraffityMountedNode mountedItem, [Context] [JoinByUser] PresetNode preset) {
            PreviewItem(mountedItem.Entity);
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<GarageItemComponent> item, [JoinAll] GarageItemsPreviewScreenNode screen) {
            PreviewItem(item.Entity);
        }

        [OnEventFire]
        public void OnModulesEnter(NodeAddedEvent e, ActiveModulesScreenNode modulesScreen, [JoinByScreen] SingleNode<GarageScreenContextComponent> contextNode) {
            PreviewItem(contextNode.component.ContextItem);
        }

        [OnEventComplete]
        public void ShellSelected(ListItemSelectedEvent e, SingleNode<ShellItemComponent> shell, [JoinByParentGroup] ICollection<WeaponNotPreviewNode> weapons,
            [JoinAll] GarageItemsScreenNode screen) {
            WeaponNotPreviewNode weaponNotPreviewNode = weapons.FirstOrDefault();

            if (weaponNotPreviewNode != null) {
                PreviewItem(weaponNotPreviewNode.Entity);
            }
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<SkinItemComponent> skin, [JoinByParentGroup] ICollection<HulLPreviewNode> hulls) {
            foreach (HulLPreviewNode hull in hulls) {
                if (hull.Entity.HasComponent<UserGroupComponent>()) {
                    PreviewItem(hull.Entity);
                    return;
                }
            }

            HulLPreviewNode hulLPreviewNode = hulls.FirstOrDefault();

            if (hulLPreviewNode != null) {
                PreviewItem(hulLPreviewNode.Entity);
            }
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<SkinItemComponent> skin, [JoinByParentGroup] ICollection<WeaponNotPreviewNode> weapons) {
            WeaponNotPreviewNode weaponNotPreviewNode = weapons.FirstOrDefault();

            if (weaponNotPreviewNode != null) {
                PreviewItem(weaponNotPreviewNode.Entity);
            }
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<ContainerMarkerComponent> container) {
            PreviewItem(container.Entity);
        }

        [OnEventFire]
        public void RevertToMountedForSkins(NodeRemoveEvent e, SingleNode<GarageItemsScreenComponent> itemsScreen, [JoinAll] PresetNode preset,
            [JoinByUser] [Combine] MountedUserSkinNode mountedItem) {
            PreviewItem(mountedItem.Entity);
        }

        [OnEventFire]
        public void RevertToMounted(NodeAddedEvent e, SingleNode<ScreenComponent> screen, [JoinAll] PresetNode preset, [JoinByUser] [Combine] NotGraffityMountedNode mountedItem) {
            if (screen.component.gameObject.GetComponent<ItemsListScreenComponent>() == null) {
                PreviewItem(mountedItem.Entity);
            }
        }

        [OnEventFire]
        public void RemovePreview(PrewievEvent e, NotGraffitiNode notGraffiti, [JoinAll] [Combine] GraffitiWithPreviewNode previewedGraffiti) {
            previewedGraffiti.Entity.RemoveComponent<HangarItemPreviewComponent>();
        }

        [OnEventComplete]
        public void ApplyPreview(PrewievEvent e, GraffitiNotPreviewNode graffiti, [JoinAll] ICollection<GraffitiPreviewNode> otherGraffiti) {
            otherGraffiti.ForEach(delegate(GraffitiPreviewNode o) {
                o.Entity.RemoveComponent<HangarItemPreviewComponent>();
            });

            graffiti.Entity.AddComponent<HangarItemPreviewComponent>();
        }

        [OnEventFire]
        public void ApplyPreview(PrewievEvent e, NotGraffitiNode node) {
            ApplyPreview(node.Entity);
        }

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

        [OnEventFire]
        public void CleanCurrentPreview(ResetPreviewEvent e, Node any, [JoinAll] ICollection<SingleNode<ContainersUI>> containersUis,
            [JoinAll] ICollection<SingleNode<HangarItemPreviewComponent>> previewItems) {
            List<Entity> except = new();

            foreach (SingleNode<ContainersUI> containersUi in containersUis) {
                if (containersUi.Entity == any) {
                    continue;
                }

                containersUi.component.GetContainers().ForEach(delegate(ContainerBoxItem cb) {
                    except.Add(cb.MarketItem);

                    if (cb.UserItem != null) {
                        except.Add(cb.UserItem);
                    }
                });
            }

            previewItems.ForEach(delegate(SingleNode<HangarItemPreviewComponent> item) {
                if (!except.Contains(item.Entity) &&
                    (!item.Entity.HasComponent<PreviewGroupComponent>() || item.Entity.GetComponent<PreviewGroupComponent>().Key != e.ExceptPreviewGroup)) {
                    item.Entity.RemoveComponent<HangarItemPreviewComponent>();
                }
            });
        }

        [OnEventComplete]
        public void DefaultItemsPreview(ResetPreviewEvent e, Node any, [JoinAll] PresetNode selectedPreset, [JoinByUser] ICollection<MountedUserItemNode> mountedItems,
            [JoinAll] ICollection<DefaultSkinItemNode> defaultSkins) {
            defaultSkins.ForEach(delegate(DefaultSkinItemNode item) {
                if (!item.Entity.HasComponent<HangarItemPreviewComponent>() &&
                    (!item.Entity.HasComponent<PreviewGroupComponent>() || item.Entity.GetComponent<PreviewGroupComponent>().Key != e.ExceptPreviewGroup)) {
                    item.Entity.AddComponent<HangarItemPreviewComponent>();
                }
            });

            mountedItems.ForEach(delegate(MountedUserItemNode item) {
                ApplyPreview(item.Entity);
            });
        }

        [OnEventComplete]
        public void ResetPreview(NodeAddedEvent e, ContainerContentScreenHidingNode screen) {
            ScheduleEvent<ResetPreviewEvent>(screen);
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class WeaponItemWithPropertiesNotPreviewNode : Node {
            public WeaponItemComponent weaponItem;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class HullItemWithPropertiesNotPreviewNode : Node {
            public TankItemComponent tankItem;
        }

        public class MountedUserSkinNode : MountedUserItemNode {
            public SkinItemComponent skinItem;
        }

        public class DefaultSkinItemNode : Node {
            public DefaultSkinItemComponent defaultSkinItem;
            public MarketItemComponent marketItem;

            public ParentGroupComponent parentGroup;
        }

        public class ActiveScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        public class ActiveModulesScreenNode : ActiveScreenNode {
            public GarageModulesScreenComponent garageModulesScreen;
        }

        public class GarageItemsPreviewScreenNode : ActiveScreenNode {
            public GarageItemsPreviewScreenComponent garageItemsPreviewScreen;
        }

        public class GarageItemsScreenNode : ActiveScreenNode {
            public GarageItemsScreenComponent garageItemsScreen;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class GraffitiNotPreviewNode : Node {
            public GraffitiItemComponent graffitiItem;
        }

        public class GraffitiWithPreviewNode : Node {
            public GraffitiItemComponent graffitiItem;

            public HangarItemPreviewComponent hangarItemPreview;
        }

        public class ContainerContentScreenHidingNode : Node {
            public ContainerContentScreenComponent containerContentScreen;

            public ScreenHidingComponent screenHiding;
        }

        public class NotGraffityMountedNode : NotGraffitiNode {
            public MountedItemComponent mountedItem;
        }

        public class PresetNode : Node {
            public PresetItemComponent presetItem;

            public SelectedPresetComponent selectedPreset;
        }
    }
}