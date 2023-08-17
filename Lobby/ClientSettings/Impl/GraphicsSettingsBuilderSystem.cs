using System.Collections.Generic;
using System.Linq;
using Lobby.ClientControls.API;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using UnityEngine;

namespace Lobby.ClientSettings.Impl {
    public class GraphicsSettingsBuilderSystem : ECSSystem {
        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        [OnEventFire]
        public void BuildSaturationGraphicsSettings(NodeAddedEvent e, SaturationLevelSettingsBuilderNode builder) =>
            BuildGraphicsSettings<SaturationLevelVariantTemplate>(builder);

        [OnEventFire]
        public void BuildSaturationLevelSettings(GraphicsSettingsBuilderReadyEvent e,
            SaturationLevelSettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<SaturationLevelVariantComponent>> saturationLevels,
            SaturationLevelSettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentSaturationLevel = GraphicsSettings.INSTANCE.CurrentSaturationLevel;

            SingleNode<SaturationLevelVariantComponent> singleNode =
                saturationLevels.OrderBy(i => Mathf.Abs(i.component.Value - currentSaturationLevel)).First();

            SingleNode<SaturationLevelVariantComponent> singleNode2 = saturationLevels
                .OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultSaturationLevel)).First();

            GraphicsSettings.INSTANCE.ApplySaturationLevel(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentSaturationLevelIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultSaturationLevelIndex = items.IndexOf(singleNode2.Entity);
        }

        [OnEventFire]
        public void BuildScreenResolutionSettings(NodeAddedEvent evt,
            SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) =>
            graphicsSettingsIndexes.component.CalculateScreenResolutionIndexes();

        [OnEventComplete]
        public void CleanBuilder(GraphicsSettingsBuilderReadyEvent e, GraphicsSettingsBuilderNode builder) {
            builder.graphicsSettingsBuilder.Items.ForEach(delegate(Entity item) {
                DeleteEntity(item);
            });

            DeleteEntity(builder.Entity);
        }

        void BuildGraphicsSettings<T>(GraphicsSettingsBuilderNode builder) where T : Template {
            GraphicsSettingsBuilderGroupComponent
                group = builder.Entity.CreateGroup<GraphicsSettingsBuilderGroupComponent>();

            builder.graphicsSettingsBuilder.Items = new List<Entity>();

            builder.configPathCollection.Collection.ForEach(delegate(string s) {
                Entity entity = CreateEntity<T>(s);
                group.Attach(entity);
                builder.graphicsSettingsBuilder.Items.Add(entity);
            });

            ScheduleEvent<GraphicsSettingsBuilderReadyEvent>(builder);
        }

        [OnEventFire]
        public void InitQualitySettingsCarouselItems(NodeAddedEvent e, QualitySettingCarouselNode qualitySettingCarousel) {
            CarouselGroupComponent carouselGroup = qualitySettingCarousel.carouselGroup;
            long itemTemplateId = qualitySettingCarousel.carousel.ItemTemplateId;
            string baseConfigPath = qualitySettingCarousel.qualitySettingsCarousel.baseConfigPath;
            List<Entity> list = new();

            for (int i = 0; i < QualitySettings.names.Length; i++) {
                if (i != Quality.ultra.Level || GraphicsSettings.INSTANCE.UltraEnabled) {
                    string configPath = baseConfigPath + QualitySettings.names[i].ToLower();
                    Entity entity = CreateEntity(itemTemplateId, configPath);
                    carouselGroup.Attach(entity);
                    list.Add(entity);
                }
            }

            CarouselItemCollectionComponent carouselItemCollectionComponent = new();
            carouselItemCollectionComponent.Items = list;
            qualitySettingCarousel.Entity.AddComponent(carouselItemCollectionComponent);
        }

        [OnEventFire]
        public void InitWindowModeSettingsCarouselItems(NodeAddedEvent e,
            WindowModeSettingCarouselNode windowModeSettingCarousel,
            SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            Entity item = CreateWindowModeItem(windowModeSettingCarousel.windowModes.Fullscreen,
                false,
                windowModeSettingCarousel.carouselGroup);

            Entity item2 = CreateWindowModeItem(windowModeSettingCarousel.windowModes.Windowed,
                true,
                windowModeSettingCarousel.carouselGroup);

            List<Entity> list = new();
            list.Add(item);
            list.Add(item2);
            List<Entity> items = list;
            graphicsSettingsIndexes.component.InitWindowModeIndexes(0, 1);
            CarouselItemCollectionComponent carouselItemCollectionComponent = new();
            carouselItemCollectionComponent.Items = items;
            CarouselItemCollectionComponent component = carouselItemCollectionComponent;
            windowModeSettingCarousel.Entity.AddComponent(component);
        }

        Entity CreateWindowModeItem(string localizedCaption, bool windowed, CarouselGroupComponent carouselGroup) {
            Entity entity = CreateEntity("WindowModeItem");
            WindowModeVariantComponent windowModeVariantComponent = new();
            windowModeVariantComponent.Windowed = windowed;
            WindowModeVariantComponent component = windowModeVariantComponent;
            entity.AddComponent(component);
            CarouselItemTextComponent carouselItemTextComponent = new();
            carouselItemTextComponent.LocalizedCaption = localizedCaption;
            CarouselItemTextComponent component2 = carouselItemTextComponent;
            entity.AddComponent(component2);
            carouselGroup.Attach(entity);
            return entity;
        }

        public class GraphicsSettingsBuilderNode : Node {
            public ConfigPathCollectionComponent configPathCollection;
            public GraphicsSettingsBuilderComponent graphicsSettingsBuilder;
        }

        public class SaturationLevelSettingsBuilderNode : GraphicsSettingsBuilderNode {
            public SaturationLevelSettingsBuilderComponent saturationLevelSettingsBuilder;
        }

        public class QualitySettingCarouselNode : Node {
            public CarouselComponent carousel;

            public CarouselGroupComponent carouselGroup;
            public QualitySettingsCarouselComponent qualitySettingsCarousel;
        }

        public class WindowModeSettingCarouselNode : Node {
            public CarouselComponent carousel;

            public CarouselGroupComponent carouselGroup;

            public WindowModesComponent windowModes;
            public WindowModeSettingsCarouselComponent windowModeSettingsCarousel;
        }
    }
}