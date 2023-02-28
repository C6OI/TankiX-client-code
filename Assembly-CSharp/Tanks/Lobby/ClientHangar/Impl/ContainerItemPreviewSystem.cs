using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class ContainerItemPreviewSystem : ItemPreviewBaseSystem {
        [OnEventFire]
        public void PreviewContainer(NodeAddedEvent e, HangarNode hangar, HangarCameraNode hangarCamera, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> completedLoading,
            [JoinAll] ICollection<ContainerMarketItemNode> containers, [JoinAll] ICollection<ContainerItemPreviewNode> previewContainers) {
            if (previewContainers.Count == 0 && containers.Count > 0) {
                PreviewItem(containers.First().Entity);
            }
        }

        [OnEventFire]
        public void HideGraffiti(NodeAddedEvent e, ContainerContentScreenNode screen, [JoinAll] GraffitiPreviewNode graffiti) {
            graffiti.Entity.RemoveComponent<HangarItemPreviewComponent>();
            screen.graffitiPreview.ResetPreview();
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, ContainerContentItemNotPreviewNode containerContantItem) {
            PreviewContainerContentItem(containerContantItem.Entity);
        }

        [OnEventFire]
        public void PreviewContent(NodeAddedEvent e, SimpleContainerContentItemPreviewNode containerContentItem) {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(containerContentItem.simpleContainerContentItem.MarketItemId);
            PreviewItem(entity);

            if (entity.HasComponent<SkinItemComponent>()) {
                ScheduleEvent<JoinParentItemEvent>(entity);
            }
        }

        [OnEventComplete]
        public void ItemSelected(JoinParentItemEvent e, SingleNode<SkinItemComponent> skin, [JoinByParentGroup] HullItemNotPreviewNode hull) {
            PreviewItem(hull.Entity);
        }

        [OnEventComplete]
        public void ItemSelected(JoinParentItemEvent e, SingleNode<SkinItemComponent> skin, [JoinByParentGroup] WeaponItemNotPreviewNode weapon) {
            PreviewItem(weapon.Entity);
        }

        [OnEventFire]
        public void UnpreviewContent(NodeRemoveEvent e, SimpleContainerContentItemPreviewNode containerContentItem, [JoinAll] ContainerContentScreenNode screen) {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(containerContentItem.simpleContainerContentItem.MarketItemId);
            ScheduleEvent<PreviewMountedItemsEvent>(entity);
        }

        [OnEventFire]
        public void RevertToMounted(PreviewMountedItemsEvent e, SingleNode<GarageItemComponent> item, [JoinAll] ContainerContentScreenNode screen,
            [JoinAll] [Combine] MountedUserItemNode mountedItem) {
            if (!mountedItem.Entity.HasComponent<GraffitiItemComponent>()) {
                PreviewItem(mountedItem.Entity);
            }
        }

        [OnEventFire]
        public void ResetGraffiti(PreviewMountedItemsEvent e, SingleNode<GraffitiItemComponent> graffiti, [JoinAll] ContainerContentScreenNode screen) {
            screen.graffitiPreview.ResetPreview();
        }

        [OnEventFire]
        public void RevertToMounted(NodeRemoveEvent e, ContainerContentScreenNode screen, [JoinAll] ContainerContentItemPreviewNode containerContentItem) {
            containerContentItem.Entity.RemoveComponent<ContainerContentItemPreviewComponent>();
        }

        public void PreviewContainerContentItem(Entity item) {
            IList<ContainerContentItemPreviewNode> list = Select<ContainerContentItemPreviewNode>(item, typeof(ContainerGroupComponent));

            list.ForEach(delegate(ContainerContentItemPreviewNode p) {
                if (p.Entity != item) {
                    p.Entity.RemoveComponent<ContainerContentItemPreviewComponent>();
                }
            });

            if (!item.HasComponent<ContainerContentItemPreviewComponent>()) {
                item.AddComponent<ContainerContentItemPreviewComponent>();
            }
        }

        public class ContainerContentScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ContainerContentScreenComponent containerContentScreen;

            public GraffitiPreviewComponent graffitiPreview;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class WeaponItemNotPreviewNode : Node {
            public MarketItemComponent marketItem;
            public WeaponItemComponent weaponItem;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class HullItemNotPreviewNode : Node {
            public MarketItemComponent marketItem;
            public TankItemComponent tankItem;
        }

        [Not(typeof(ContainerContentItemPreviewComponent))]
        public class ContainerContentItemNotPreviewNode : Node {
            public ContainerContentItemComponent containerContentItem;
        }

        public class ContainerContentItemPreviewNode : Node {
            public ContainerContentItemComponent containerContentItem;

            public ContainerContentItemPreviewComponent containerContentItemPreview;
        }

        public class ContainerMarketItemNode : Node {
            public ContainerMarkerComponent containerMarker;

            public MarketItemComponent marketItem;

            public ResourceDataComponent resourceData;
        }

        public class ContainerItemPreviewNode : Node {
            public ContainerMarkerComponent containerMarker;

            public HangarItemPreviewComponent hangarItemPreview;
        }

        public class SimpleContainerContentItemPreviewNode : ContainerContentItemPreviewNode {
            public SimpleContainerContentItemComponent simpleContainerContentItem;
        }

        public class HangarNode : Node {
            public HangarContainerPositionComponent hangarContainerPosition;
            public HangarTankPositionComponent hangarTankPosition;
        }

        public class HangarCameraNode : Node {
            public CameraComponent camera;

            public HangarComponent hangar;
        }

        public class PreviewMountedItemsEvent : Event { }

        public class JoinParentItemEvent : Event { }
    }
}