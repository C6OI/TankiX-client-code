using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientControls.Impl {
    public class CarouselSystem : ECSSystem {
        [OnEventFire]
        public void InitCarouselButtons(NodeAddedEvent e, SingleNode<CarouselComponent> carousel) {
            Entity entity = carousel.Entity;
            CarouselGroupComponent carouselGroupComponent = entity.CreateGroup<CarouselGroupComponent>();
            CarouselComponent component = carousel.component;
            long key = carouselGroupComponent.Key;
            CarouselButtonComponent backButton = component.BackButton;
            CarouselButtonComponent frontButton = component.FrontButton;
            backButton.Build(CreateEntity(backButton.name), key);
            frontButton.Build(CreateEntity(frontButton.name), key);
        }

        [OnEventFire]
        public void InitCarouselItems(NodeAddedEvent e, CarouselConstructorNode carousel) {
            CarouselGroupComponent carouselGroup = carousel.carouselGroup;
            ConfigPathCollectionComponent configPathCollection = carousel.configPathCollection;
            List<string> collection = configPathCollection.Collection;
            long itemTemplateID = carousel.carousel.ItemTemplateId;
            List<Entity> items = new();

            collection.ForEach(delegate(string itemTemplatePath) {
                Entity entity = CreateEntity(itemTemplateID, itemTemplatePath);
                carouselGroup.Attach(entity);
                items.Add(entity);
            });

            CarouselItemCollectionComponent carouselItemCollectionComponent = new();
            carouselItemCollectionComponent.Items = items;
            carousel.Entity.AddComponent(carouselItemCollectionComponent);
        }

        [OnEventFire]
        public void ClearCarouselItems(NodeRemoveEvent e, SingleNode<CarouselItemCollectionComponent> carousel) {
            carousel.component.Items.ForEach(delegate(Entity item) {
                DeleteEntity(item);
            });
        }

        [OnEventFire]
        public void InitCarouselButton(NodeAddedEvent e, SingleNode<CarouselBackButtonComponent> button) {
            button.Entity.AddComponent(new CarouselGroupComponent(button.component.CarouselEntity));
        }

        [OnEventFire]
        public void InitCarouselButton(NodeAddedEvent e, SingleNode<CarouselFrontButtonComponent> button) {
            button.Entity.AddComponent(new CarouselGroupComponent(button.component.CarouselEntity));
        }

        [OnEventFire]
        public void ClearCarouselButtons(NodeRemoveEvent e, SingleNode<CarouselComponent> carousel) {
            CarouselComponent component = carousel.component;
            CarouselButtonComponent backButton = component.BackButton;
            CarouselButtonComponent frontButton = component.FrontButton;
            backButton.DestroyButton();
            frontButton.DestroyButton();
        }

        [OnEventFire]
        public void InitFirstCarouselItem(NodeAddedEvent e, ReadyCarouselNode carousel) {
            List<Entity> items = carousel.carouselItemCollection.Items;
            int index = carousel.carouselCurrentItemIndex.Index;
            Entity entity = items[index];
            entity.AddComponent<CarouselCurrentItemComponent>();
        }

        [OnEventFire]
        public void UpdateCarouselText(NodeAddedEvent e, CarouselCurrentItemNode item, [JoinByCarousel] ReadyCarouselNode carousel) {
            carousel.carousel.Text.text = item.carouselItemText.LocalizedCaption;
        }

        [OnEventFire]
        public void SwitchCarouselItem(CarouselItemBeforeChangeEvent evt, ReadyCarouselNode carousel, CarouselItemNode item, [JoinByCarousel] CarouselCurrentItemNode currentItem) {
            currentItem.Entity.RemoveComponent<CarouselCurrentItemComponent>();
            item.Entity.AddComponent<CarouselCurrentItemComponent>();
            NewEvent<CarouselItemChangedEvent>().AttachAll(carousel, item).Schedule();
        }

        [OnEventFire]
        public void ClickFront(ButtonClickEvent e, SingleNode<CarouselFrontButtonComponent> btn, [JoinByCarousel] ReadyCarouselNode carousel) {
            MoveCarousel(carousel, 1);
        }

        [OnEventFire]
        public void ClickBack(ButtonClickEvent e, SingleNode<CarouselBackButtonComponent> btn, [JoinByCarousel] ReadyCarouselNode carousel) {
            MoveCarousel(carousel, -1);
        }

        [OnEventFire]
        public void SetCarouselItemIndex(SetCarouselItemIndexEvent e, ReadyCarouselNode carousel) {
            int index = e.Index;
            carousel.carouselCurrentItemIndex.Index = index;
            int index2 = index;
            Entity entity = carousel.carouselItemCollection.Items[index2];
            NewEvent<CarouselItemBeforeChangeEvent>().AttachAll(carousel.Entity, entity).Schedule();
        }

        void MoveCarousel(ReadyCarouselNode carousel, int dir) {
            List<Entity> items = carousel.carouselItemCollection.Items;
            int count = items.Count;
            int index = count - 1;
            CarouselCurrentItemIndexComponent carouselCurrentItemIndex = carousel.carouselCurrentItemIndex;
            carouselCurrentItemIndex.Index += dir;

            if (carouselCurrentItemIndex.Index >= count) {
                carouselCurrentItemIndex.Index = 0;
            } else if (carouselCurrentItemIndex.Index < 0) {
                carouselCurrentItemIndex.Index = index;
            }

            Entity entity = items[carouselCurrentItemIndex.Index];
            NewEvent<CarouselItemBeforeChangeEvent>().AttachAll(carousel.Entity, entity).Schedule();
        }

        public class CarouselConstructorNode : Node {
            public CarouselComponent carousel;

            public CarouselGroupComponent carouselGroup;

            public ConfigPathCollectionComponent configPathCollection;
        }

        public class ReadyCarouselNode : Node {
            public CarouselComponent carousel;

            public CarouselCurrentItemIndexComponent carouselCurrentItemIndex;

            public CarouselItemCollectionComponent carouselItemCollection;
        }

        public class CarouselItemNode : Node {
            public CarouselItemTextComponent carouselItemText;
        }

        public class CarouselCurrentItemNode : CarouselItemNode {
            public CarouselCurrentItemComponent carouselCurrentItem;
        }
    }
}