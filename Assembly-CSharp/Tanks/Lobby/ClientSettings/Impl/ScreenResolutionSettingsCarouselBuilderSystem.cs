using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.Impl {
    public class ScreenResolutionSettingsCarouselBuilderSystem : ECSSystem {
        const string SCREEN_RESOLUTION_ENTITY_NAME = "ScreenResolutionEntity";

        const string SCREEN_RESOLUTION_CAPTION_FORMAT = "{0} x {1}";

        [OnEventFire]
        public void Build(NodeAddedEvent e, ScreenResolutionSettingsCarouselNode carousel) {
            CarouselGroupComponent carouselGroup = carousel.carouselGroup;
            GraphicsSettings iNSTANCE = GraphicsSettings.INSTANCE;
            List<Resolution> screenResolutions = iNSTANCE.ScreenResolutions;
            int count = screenResolutions.Count;
            List<Entity> list = new();

            for (int i = 0; i < count; i++) {
                Resolution resolution = screenResolutions[i];
                int width = resolution.width;
                int height = resolution.height;
                Entity entity = CreateEntity("ScreenResolutionEntity");
                entity.AddComponentAndGetInstance<CarouselItemTextComponent>().LocalizedCaption = string.Format("{0} x {1}", width, height);
                ScreenResolutionVariantComponent screenResolutionVariantComponent = entity.AddComponentAndGetInstance<ScreenResolutionVariantComponent>();
                screenResolutionVariantComponent.Width = width;
                screenResolutionVariantComponent.Height = height;
                carouselGroup.Attach(entity);
                list.Add(entity);
            }

            CarouselItemCollectionComponent carouselItemCollectionComponent = new();
            carouselItemCollectionComponent.Items = list;
            carousel.Entity.AddComponent(carouselItemCollectionComponent);
        }

        public class ScreenResolutionSettingsCarouselNode : Node {
            public CarouselComponent carousel;

            public CarouselGroupComponent carouselGroup;

            public ScreenResolutionSettingsCarouselComponent screenResolutionSettingsCarousel;
        }
    }
}