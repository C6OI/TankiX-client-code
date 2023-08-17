using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientSettings.Impl {
    public class GraphicsSettingsScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitQualitySettingsCarousel(NodeAddedEvent e, QualitySettingsCarouselNode carousel,
            [Context] [JoinByScreen] GraphicsSettingsScreenNode screen) =>
            carousel.Entity.AddComponent(
                new CarouselCurrentItemIndexComponent(GraphicsSettings.INSTANCE.CurrentQualityLevel));

        [OnEventFire]
        public void InitScreenResolutionSettingsCarousel(NodeAddedEvent e, ScreenResolutionSettingsCarouselNode carousel,
            [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) => carousel.Entity.AddComponent(
            new CarouselCurrentItemIndexComponent(graphicsSettingsIndexes.component.CurrentScreenResolutionIndex));

        [OnEventFire]
        public void InitSaturationLevelCarousel(NodeAddedEvent e, SaturationLevelCarouselNode carousel,
            [JoinByScreen] [Context] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) => carousel.Entity.AddComponent(
            new CarouselCurrentItemIndexComponent(graphicsSettingsIndexes.component.CurrentSaturationLevelIndex));

        [OnEventFire]
        public void InitWindowModeSettingsCarousel(NodeAddedEvent e, WindowModeSettingsCarouselNode carousel,
            [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            carousel.windowModeSettingsCarousel.FullScreen = Screen.fullScreen;

            carousel.Entity.AddComponent(
                new CarouselCurrentItemIndexComponent(graphicsSettingsIndexes.component.CurrentWindowModeIndex));
        }

        [OnEventFire]
        public void InitDataFromCarousels(NodeAddedEvent e, GraphicsSettingsScreenNode screen,
            [Context] [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel,
            [JoinByScreen] [Context] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            [Context] [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel,
            [Context] [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) => UpdateRightMenuElements(
            qualityCarousel,
            screenResolutionCarousel,
            windowModeSettingsCarousel,
            saturationLevelSettingsCarousel,
            screen,
            graphicsSettingsIndexes);

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyQualitySettingsCarouselNode carousel,
            [JoinByScreen] GraphicsSettingsScreenNode screen) => ScheduleEvent<GraphicsSettingsChangedEvent>(screen);

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyScreenResolutionSettingsCarouselNode carousel,
            [JoinByScreen] GraphicsSettingsScreenNode screen) => ScheduleEvent<GraphicsSettingsChangedEvent>(screen);

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyWindowModeSettingsCarouselNode carousel,
            [JoinByScreen] GraphicsSettingsScreenNode screen) => ScheduleEvent<GraphicsSettingsChangedEvent>(screen);

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadySaturationLevelSettingsCarouselNode carousel,
            [JoinByScreen] GraphicsSettingsScreenNode screen, ReadySaturationLevelSettingsCarouselNode carousel1,
            [JoinByCarousel] CurrentSaturationLevelCarouselItemNode saturationLevelCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplySaturationLevel(saturationLevelCarouselItem.saturationLevelVariant.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void DetectFullScreenChange(UpdateEvent e, ReadyWindowModeSettingsCarouselNode carousel,
            [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            bool fullScreen = Screen.fullScreen;

            if (carousel.windowModeSettingsCarousel.FullScreen != fullScreen) {
                ScheduleEvent(new SetCarouselItemIndexEvent(graphicsSettingsIndexes.component.CurrentWindowModeIndex),
                    carousel);
            }

            carousel.windowModeSettingsCarousel.FullScreen = fullScreen;
        }

        [OnEventFire]
        public void ChangeCarouselItem(GraphicsSettingsChangedEvent e, GraphicsSettingsScreenNode screen,
            [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel,
            [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel,
            [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) => UpdateRightMenuElements(
            qualityCarousel,
            screenResolutionCarousel,
            windowModeSettingsCarousel,
            saturationLevelSettingsCarousel,
            screen,
            graphicsSettingsIndexes);

        [OnEventFire]
        public void Cancel(ButtonClickEvent e, SingleNode<CancelButtonComponent> button,
            [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel,
            [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeCarousel,
            [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            ScheduleEvent(new SetCarouselItemIndexEvent(GraphicsSettings.INSTANCE.CurrentQualityLevel), qualityCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentScreenResolutionIndex), screenResolutionCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentWindowModeIndex), windowModeCarousel);

            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentSaturationLevelIndex),
                saturationLevelSettingsCarousel);
        }

        [OnEventFire]
        public void SetDefault(ButtonClickEvent e, SingleNode<DefaultButtonComponent> button,
            [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel,
            [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeCarousel,
            [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            ScheduleEvent(new SetCarouselItemIndexEvent(GraphicsSettings.INSTANCE.DefaultQualityLevel), qualityCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultScreenResolutionIndex), screenResolutionCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultWindowModeIndex), windowModeCarousel);

            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultSaturationLevelIndex),
                saturationLevelSettingsCarousel);
        }

        [OnEventFire]
        public void Apply(ButtonClickEvent e, SingleNode<ApplyButtonComponent> button,
            [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel,
            [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel,
            [JoinByCarousel] CurrentWindowModeCarouselItemNode windowMode, SingleNode<ApplyButtonComponent> button1,
            [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            [JoinByCarousel] CurrentScreenResolutionCarouselItemNode screenResolutionItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings iNSTANCE = GraphicsSettings.INSTANCE;
            bool windowed = windowMode.windowModeVariant.Windowed;
            iNSTANCE.ApplyWindowMode(windowed);
            windowModeSettingsCarousel.windowModeSettingsCarousel.FullScreen = !windowed;
            ScreenResolutionVariantComponent screenResolutionVariant = screenResolutionItem.screenResolutionVariant;
            int resolutionWidth = screenResolutionVariant.Width;
            int resolutionHeight = screenResolutionVariant.Height;
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;

            component.CurrentScreenResolutionIndex =
                iNSTANCE.ScreenResolutions.FindIndex(r => r.width == resolutionWidth && r.height == resolutionHeight);

            iNSTANCE.ApplyScreenResolution(resolutionWidth, resolutionHeight, windowed);

            graphicsSettingsIndexes.component.CurrentSaturationLevelIndex =
                saturationLevelSettingsCarousel.carouselCurrentItemIndex.Index;

            if (!screen.graphicsSettingsScreen.NeedToReloadApplication) {
                ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
                return;
            }

            int index = qualityCarousel.carouselCurrentItemIndex.Index;
            iNSTANCE.ApplyQualityLevel(index);
            ScheduleEvent<SwitchToEntranceSceneEvent>(button);
        }

        void UpdateRightMenuElements(ReadyQualitySettingsCarouselNode qualityCarousel,
            ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel,
            ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel, GraphicsSettingsScreenNode screen,
            SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettingsScreenComponent graphicsSettingsScreen = screen.graphicsSettingsScreen;
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            int index = qualityCarousel.carouselCurrentItemIndex.Index;
            int index2 = screenResolutionCarousel.carouselCurrentItemIndex.Index;
            int index3 = windowModeSettingsCarousel.carouselCurrentItemIndex.Index;
            int index4 = saturationLevelSettingsCarousel.carouselCurrentItemIndex.Index;
            bool needToShowChangePerfomance = index > GraphicsSettings.INSTANCE.DefaultQualityLevel;
            bool isCurrentQuality = index == GraphicsSettings.INSTANCE.CurrentQualityLevel;
            bool flag = index != GraphicsSettings.INSTANCE.CurrentQualityLevel;

            bool needToShowButtons = index4 != component.CurrentSaturationLevelIndex ||
                                     index3 != component.CurrentWindowModeIndex ||
                                     index2 != component.CurrentScreenResolutionIndex ||
                                     flag;

            bool defaultButtonVisibility = index != GraphicsSettings.INSTANCE.DefaultQualityLevel ||
                                           index2 != component.DefaultScreenResolutionIndex ||
                                           index3 != component.DefaultWindowModeIndex ||
                                           index4 != component.DefaultSaturationLevelIndex;

            graphicsSettingsScreen.SetPerfomanceWarningVisibility(needToShowChangePerfomance, isCurrentQuality);
            graphicsSettingsScreen.SetVisibilityForChangeSettingsControls(flag, needToShowButtons);
            graphicsSettingsScreen.SetDefaultButtonVisibility(defaultButtonVisibility);
        }

        public class GraphicsSettingsScreenNode : Node {
            public GraphicsSettingsScreenComponent graphicsSettingsScreen;

            public ScreenGroupComponent screenGroup;
        }

        public class CarouselNode : Node {
            public CarouselComponent carousel;

            public CarouselItemCollectionComponent carouselItemCollection;

            public ScreenGroupComponent screenGroup;
        }

        public class ReadyCarouselNode : CarouselNode {
            public CarouselCurrentItemIndexComponent carouselCurrentItemIndex;
        }

        public class QualitySettingsCarouselNode : CarouselNode {
            public QualitySettingsCarouselComponent qualitySettingsCarousel;
        }

        public class ScreenResolutionSettingsCarouselNode : CarouselNode {
            public ScreenResolutionSettingsCarouselComponent screenResolutionSettingsCarousel;
        }

        public class WindowModeSettingsCarouselNode : CarouselNode {
            public WindowModeSettingsCarouselComponent windowModeSettingsCarousel;
        }

        public class SaturationLevelCarouselNode : CarouselNode {
            public SaturationLevelCarouselComponent saturationLevelCarousel;
        }

        public class ReadyQualitySettingsCarouselNode : ReadyCarouselNode {
            public QualitySettingsCarouselComponent qualitySettingsCarousel;
        }

        public class ReadyScreenResolutionSettingsCarouselNode : ReadyCarouselNode {
            public ScreenResolutionSettingsCarouselComponent screenResolutionSettingsCarousel;
        }

        public class ReadyWindowModeSettingsCarouselNode : ReadyCarouselNode {
            public WindowModeSettingsCarouselComponent windowModeSettingsCarousel;
        }

        public class ReadySaturationLevelSettingsCarouselNode : ReadyCarouselNode {
            public SaturationLevelCarouselComponent saturationLevelCarousel;
        }

        public class CurrentCarouselItemNode : Node {
            public CarouselCurrentItemComponent carouselCurrentItem;
        }

        public class CurrentScreenResolutionCarouselItemNode : CurrentCarouselItemNode {
            public ScreenResolutionVariantComponent screenResolutionVariant;
        }

        public class CurrentWindowModeCarouselItemNode : CurrentCarouselItemNode {
            public WindowModeVariantComponent windowModeVariant;
        }

        public class CurrentSaturationLevelCarouselItemNode : CurrentCarouselItemNode {
            public SaturationLevelVariantComponent saturationLevelVariant;
        }
    }
}