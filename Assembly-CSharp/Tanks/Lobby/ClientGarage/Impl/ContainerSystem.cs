using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ContainerSystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void CreateContainerGroup(NodeAddedEvent e, ContainerMarketItemNode container) {
            container.Entity.CreateGroup<ContainerGroupComponent>();
        }

        [OnEventFire]
        public void JoinUserContainerToMarketContainer(NodeAddedEvent e, ContainerUserItemNode userContainer,
            [JoinByMarketItem] Optional<ContainerMarketItemWithGroupNode> marketContainer) {
            if (marketContainer.IsPresent()) {
                marketContainer.Get().containerGroup.Attach(userContainer.Entity);
            } else {
                userContainer.Entity.CreateGroup<ContainerGroupComponent>();
            }
        }

        [OnEventFire]
        public void CreateContainerContentItems(NodeAddedEvent e, ContainerItemWithGroupNode container,
            [JoinByContainer] ICollection<SingleNode<ContainerContentItemComponent>> containerContentItems) {
            if (containerContentItems.Count <= 0) {
                CreateContainerContentItems(container.itemsContainerItem.Items, container, false);

                if (container.itemsContainerItem.RareItems != null && container.itemsContainerItem.RareItems.Count > 0) {
                    CreateContainerContentItems(container.itemsContainerItem.RareItems, container, true);
                }
            }
        }

        void CreateContainerContentItems(List<ContainerItem> containerItems, ContainerItemWithGroupNode container, bool isRare) {
            int num = 0;

            if (isRare) {
                num = 100;
            }

            foreach (ContainerItem containerItem in containerItems) {
                Entity entity = CreateEntity("ContainerContentItem");
                entity.AddComponent<ContainerContentItemComponent>();
                OrderItemComponent orderItemComponent = new();
                num = orderItemComponent.Index = num + 1;
                entity.AddComponent(orderItemComponent);

                if (isRare) {
                    entity.AddComponent<RareContainerContentItemComponent>();
                }

                DescriptionBundleItemComponent descriptionBundleItemComponent = new();
                descriptionBundleItemComponent.Names = container.descriptionBundleItem.Names;
                DescriptionBundleItemComponent component = descriptionBundleItemComponent;
                entity.AddComponent(component);

                if (containerItem.ItemBundles.Count == 1) {
                    SimpleContainerContentItemComponent simpleContainerContentItemComponent = new();
                    simpleContainerContentItemComponent.MarketItemId = containerItem.ItemBundles[0].MarketItem;
                    simpleContainerContentItemComponent.NameLokalizationKey = containerItem.NameLocalizationKey;
                    SimpleContainerContentItemComponent component2 = simpleContainerContentItemComponent;
                    entity.AddComponent(component2);
                } else {
                    BundleContainerContentItemComponent bundleContainerContentItemComponent = new();
                    bundleContainerContentItemComponent.MarketItems = containerItem.ItemBundles;
                    bundleContainerContentItemComponent.NameLokalizationKey = containerItem.NameLocalizationKey;
                    BundleContainerContentItemComponent component3 = bundleContainerContentItemComponent;
                    entity.AddComponent(component3);
                }

                entity.CreateGroup<ContainerContentItemGroupComponent>();
                container.containerGroup.Attach(entity);
            }
        }

        [OnEventFire]
        public void JoinMarketItem(NodeAddedEvent e, [Combine] SingleNode<MarketItemComponent> marketItemNode, [Combine] SimpleContainerContentItemNode containerContentItemNode,
            [JoinByContainer] ContainerMarketItemWithGroupNode container) {
            if (!containerContentItemNode.simpleContainerContentItem.MarketItemId.Equals(marketItemNode.Entity.Id)) {
                return;
            }

            if (marketItemNode.Entity.HasComponent<ContainerContentItemGroupComponent>()) {
                int count = container.itemsContainerItem.Items.Count;
                Entity entity = Flow.Current.EntityRegistry.GetEntity(marketItemNode.Entity.GetComponent<ContainerContentItemGroupComponent>().Key);
                JoinContainerItemEvent joinContainerItemEvent = new();
                ScheduleEvent(joinContainerItemEvent, entity);
                int count2 = joinContainerItemEvent.ContainerEntity.GetComponent<ItemsContainerItemComponent>().Items.Count;

                if (count < count2) {
                    entity.GetComponent<ContainerContentItemGroupComponent>().Detach(marketItemNode.Entity);
                    containerContentItemNode.containerContentItemGroup.Attach(marketItemNode.Entity);
                }
            } else {
                containerContentItemNode.containerContentItemGroup.Attach(marketItemNode.Entity);
            }
        }

        [OnEventFire]
        public void JoinContainerItem(JoinContainerItemEvent e, SimpleContainerContentItemNode containerContentItemNode, [JoinByContainer] ContainerMarketItemWithGroupNode container) {
            e.ContainerEntity = container.Entity;
        }

        public class ContainerItemNode : Node {
            public DescriptionBundleItemComponent descriptionBundleItem;

            public DescriptionItemComponent descriptionItem;

            public ImageItemComponent imageItem;
            public ItemsContainerItemComponent itemsContainerItem;

            public MarketItemGroupComponent marketItemGroup;
        }

        public class ContainerMarketItemNode : ContainerItemNode {
            public MarketItemComponent marketItem;
        }

        public class ContainerUserItemNode : ContainerItemNode {
            public UserItemComponent userItem;
        }

        public class ContainerMarketItemWithGroupNode : ContainerMarketItemNode {
            public ContainerGroupComponent containerGroup;
        }

        public class ContainerItemWithGroupNode : ContainerItemNode {
            public ContainerGroupComponent containerGroup;
        }

        public class SimpleContainerContentItemNode : Node {
            public ContainerContentItemComponent containerContentItem;

            public ContainerContentItemGroupComponent containerContentItemGroup;

            public ContainerGroupComponent containerGroup;

            public SimpleContainerContentItemComponent simpleContainerContentItem;
        }

        public class JoinContainerItemEvent : Event {
            public Entity ContainerEntity { get; set; }
        }
    }
}