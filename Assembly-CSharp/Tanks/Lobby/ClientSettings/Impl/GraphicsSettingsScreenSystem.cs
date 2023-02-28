using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientSettings.Impl {
    public class GraphicsSettingsScreenSystem : ECSSystem {
        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, CustomSettingsCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.CustomSettings;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, CustomSettingsCheckboxNode checkboxNode, AmbientOcclusionCheckboxNode AmbientOcclusionCheckbox,
            BloomCheckboxNode BloomCheckbox, ChromaticAberrationCheckboxNode ChromaticAberrationCheckbox, GrainCheckboxNode GrainCheckbox, VignetteCheckboxNode VignetteCheckbox,
            [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            AmbientOcclusionCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            BloomCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ChromaticAberrationCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            GrainCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            VignetteCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AmbientOcclusionCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            BloomCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            ChromaticAberrationCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            GrainCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            VignetteCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, AmbientOcclusionCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.AmbientOcclusion;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, BloomCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.Bloom;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, ChromaticAberrationCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.ChromaticAberration;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, GrainCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.Grain;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, VignetteCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.Vignette;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, DisableBattleNotificationsCheckboxNode checkboxNode,
            [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.DisableBattleNotifications;
        }

        [OnEventFire]
        public void ChangeCustomSettings(CheckboxEvent e, CustomSettingsCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings,
            [JoinAll] AmbientOcclusionCheckboxNode AmbientOcclusionCheckbox, [JoinAll] BloomCheckboxNode BloomCheckbox,
            [JoinAll] ChromaticAberrationCheckboxNode ChromaticAberrationCheckbox, [JoinAll] GrainCheckboxNode GrainCheckbox, [JoinAll] VignetteCheckboxNode VignetteCheckbox,
            [JoinAll] DisableBattleNotificationsCheckboxNode battleNotificationsCheckbox) {
            settings.component.CustomSettings = checkboxNode.checkbox.IsChecked;
            AmbientOcclusionCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            BloomCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ChromaticAberrationCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            GrainCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            VignetteCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AmbientOcclusionCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            BloomCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            ChromaticAberrationCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            GrainCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            VignetteCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            battleNotificationsCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, AmbientOcclusionCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            settings.component.AmbientOcclusion = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, BloomCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            settings.component.Bloom = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, ChromaticAberrationCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            settings.component.ChromaticAberration = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, GrainCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            settings.component.Grain = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, VignetteCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            settings.component.Vignette = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, DisableBattleNotificationsCheckboxNode checkboxNode,
            [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            settings.component.DisableBattleNotifications = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, CustomSettingsCheckboxNode checkboxNode, TextureQualityCarouselNode TextureQualityCarousel,
            ShadowQualityCarouselNode ShadowQualityCarousel, ParticleQualityCarouselNode ParticleQualityCarousel, VegetationQualityCarouselNode VegetationQualityCarousel,
            GrassQualityCarouselNode GrassQualityCarousel, AntialiasingQualityCarouselNode AntialiasingQualityCarousel,
            RenderResolutionQualityCarouselNode RenderResolutionQualityCarousel, AnisotropicQualityCarouselNode AnisotropicQualityCarousel,
            CartridgeCaseAmountCarouselNode cartridgeCaseAmountCarousel, VSyncQualityCarouselNode vsyncQualityCarousel,
            [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            TextureQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ShadowQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ParticleQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            VegetationQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            GrassQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AntialiasingQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            RenderResolutionQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AnisotropicQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            cartridgeCaseAmountCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            vsyncQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            TextureQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            ShadowQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            ParticleQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            VegetationQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            GrassQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            AntialiasingQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            RenderResolutionQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            AnisotropicQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            cartridgeCaseAmountCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            vsyncQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
        }

        [OnEventFire]
        public void InitQualitySettingsCarousel(NodeAddedEvent e, QualitySettingsCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(GraphicsSettings.INSTANCE.CurrentQualityLevel));
        }

        [OnEventFire]
        public void InitScreenResolutionSettingsCarousel(NodeAddedEvent e, ScreenResolutionSettingsCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingsIndexes.component.CurrentScreenResolutionIndex));
        }

        [OnEventFire]
        public void InitSaturationLevelCarousel(NodeAddedEvent e, SaturationLevelCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingsIndexes.component.CurrentSaturationLevelIndex));
        }

        [OnEventFire]
        public void InitVegetationQualityCarousel(NodeAddedEvent e, VegetationQualityCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentVegetationQualityIndex));
        }

        [OnEventFire]
        public void InitGrassQualityCarousel(NodeAddedEvent e, GrassQualityCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentGrassQualityIndex));
        }

        [OnEventFire]
        public void InitCartridgeAmountCarousel(NodeAddedEvent e, CartridgeCaseAmountCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentCartridgeCaseAmountIndex));
        }

        [OnEventFire]
        public void InitVSyncQualityCarousel(NodeAddedEvent e, VSyncQualityCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentVSyncQualityIndex));
        }

        [OnEventFire]
        public void InitRenderResolutionQualityCarousel(NodeAddedEvent e, RenderResolutionQualityCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentRenderResolutionQualityIndex));
        }

        [OnEventFire]
        public void InitAntialiasingQualityCarousel(NodeAddedEvent e, AntialiasingQualityCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentAntialiasingQualityIndex));
        }

        [OnEventFire]
        public void InitAnisotropicQualityCarousel(NodeAddedEvent e, AnisotropicQualityCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentAnisotropicQualityIndex));
        }

        [OnEventFire]
        public void InitTextureQualityCarousel(NodeAddedEvent e, TextureQualityCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentTextureQualityIndex));
        }

        [OnEventFire]
        public void InitShadowQualityCarousel(NodeAddedEvent e, ShadowQualityCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentShadowQualityIndex));
        }

        [OnEventFire]
        public void InitParticleQualityCarousel(NodeAddedEvent e, ParticleQualityCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes) {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentParticleQualityIndex));
        }

        [OnEventFire]
        public void InitWindowModeSettingsCarousel(NodeAddedEvent e, WindowModeSettingsCarouselNode carousel, [Context] [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            carousel.windowModeSettingsCarousel.FullScreen = Screen.fullScreen;
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingsIndexes.component.CurrentWindowModeIndex));
        }

        [OnEventFire]
        public void InitDataFromCarousels(NodeAddedEvent e, GraphicsSettingsScreenNode screen, [Context] [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel,
            [Context] [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            [Context] [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel,
            [Context] [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            [Context] [JoinByScreen] ReadyVegetationQualityCarouselNode vegetationLevelSettingsCarousel, [Context] [JoinByScreen] ReadyGrassQualityCarouselNode grassLevelSettingsCarousel,
            [Context] [JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualitySettingCarousel,
            [Context] [JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel,
            [Context] [JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel,
            [Context] [JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel, [Context] [JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel,
            [Context] [JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel, [Context] [JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel,
            [Context] [JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualitCarousel, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            UpdateRightMenuElements(qualityCarousel, screenResolutionCarousel, windowModeSettingsCarousel, saturationLevelSettingsCarousel, antialiasingQualitySettingCarousel,
                renderResolutionQualityCarousel, vegetationLevelSettingsCarousel, grassLevelSettingsCarousel, anisotropicQualitySettingCarousel, shadowQualitySettingCarousel,
                particleQualitySettingCarousel, textureQualitySettingCarousel, cartridgeAmountCarousel, vsyncQualitCarousel, screen, graphicsSettingsIndexes);
        }

        [OnEventFire]
        public void ChangeCustomSettings(CheckboxEvent e, CustomSettingsCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings,
            [JoinAll] TextureQualityCarouselNode TextureQualityCarousel, [JoinAll] ShadowQualityCarouselNode ShadowQualityCarousel,
            [JoinAll] ParticleQualityCarouselNode ParticleQualityCarousel, [JoinAll] VegetationQualityCarouselNode VegetationQualityCarousel,
            [JoinAll] GrassQualityCarouselNode GrassQualityCarousel, [JoinAll] AntialiasingQualityCarouselNode AntialiasingQualityCarousel,
            [JoinAll] RenderResolutionQualityCarouselNode RenderResolutionQualityCarousel, [JoinAll] AnisotropicQualityCarouselNode AnisotropicQualityCarousel,
            [JoinAll] CartridgeCaseAmountCarouselNode cartridgeCaseAmountCarousel, [JoinAll] VSyncQualityCarouselNode vsyncQualityCarousel) {
            settings.component.CustomSettings = checkboxNode.checkbox.IsChecked;
            TextureQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ShadowQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ParticleQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            VegetationQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            GrassQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AntialiasingQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            RenderResolutionQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AnisotropicQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            cartridgeCaseAmountCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            vsyncQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            TextureQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            ShadowQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            ParticleQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            VegetationQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            GrassQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            AntialiasingQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            RenderResolutionQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            AnisotropicQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            cartridgeCaseAmountCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            vsyncQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyQualitySettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen) {
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyScreenResolutionSettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen) {
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyWindowModeSettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen) {
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadySaturationLevelSettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadySaturationLevelSettingsCarouselNode carousel1, [JoinByCarousel] CurrentSaturationLevelCarouselItemNode saturationLevelCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplySaturationLevel(saturationLevelCarouselItem.saturationLevelVariant.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyVegetationQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyVegetationQualityCarouselNode carousel1, [JoinByCarousel] CurrentVegetationQualityCarouselItemNode vegetationQualityCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplyVegetationLevel(vegetationQualityCarouselItem.vegetationSettings.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyGrassQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyGrassQualityCarouselNode carousel1, [JoinByCarousel] CurrentGrassQualityCarouselItemNode grassQualityCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplyGrassLevel(grassQualityCarouselItem.grassSettings.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyAntialiasingQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyAntialiasingQualityCarouselNode carousel1, [JoinByCarousel] CurrentAntialiasingQualityCarouselItemNode antialiasingQualityCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplyAntialiasingQuality(antialiasingQualityCarouselItem.antialiasingQualityVariant.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyRenderResolutionQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyRenderResolutionQualityCarouselNode carousel1, [JoinByCarousel] CurrentRenderResolutionQualityCarouselItemNode renderResolutionQualityCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplyRenderResolutionQuality(renderResolutionQualityCarouselItem.renderResolutionQualityVariant.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyAnisotropicQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyAnisotropicQualityCarouselNode carousel1, [JoinByCarousel] CurrentAnisotropicQualityCarouselItemNode anisotropicQualityCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplyAnisotropicQuality(anisotropicQualityCarouselItem.anisotropicQualityVariant.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyShadowQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyShadowQualityCarouselNode carousel1, [JoinByCarousel] CurrentShadowQualityCarouselItemNode shadowQualityCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplyShadowQuality(shadowQualityCarouselItem.shadowQualityVariant.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyParticleQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyParticleQualityCarouselNode carousel1, [JoinByCarousel] CurrentParticleQualityCarouselItemNode particleQualityCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplyParticleQuality(particleQualityCarouselItem.particleQualityVariant.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyTextureQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyTextureQualityCarouselNode carousel1, [JoinByCarousel] CurrentTextureQualityCarouselItemNode textureQualityCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.ApplyTextureQuality(textureQualityCarouselItem.textureQualityVariant.Value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyCartridgeAmountCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyCartridgeAmountCarouselNode carousel1, [JoinByCarousel] CurrentCartridgeCaseAmountCarouselItemNode caseAmountCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            int value = caseAmountCarouselItem.cartridgeCaseSettingVariant.Value;
            GraphicsSettings.INSTANCE.ApplyCartridgeCaseAmount(value);
            graphicsSettingsIndexes.component.CurrentCartridgeCaseAmountIndex = value;
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyVSyncQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            ReadyVSyncQualityCarouselNode carousel1, [JoinByCarousel] CurrentVSyncCarouselItemNode vsyncQualityCarouselItem,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            int value = vsyncQualityCarouselItem.vSyncSettingVariant.Value;
            GraphicsSettings.INSTANCE.ApplyVSyncQuality(value);
            ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void DetectFullScreenChange(UpdateEvent e, ReadyWindowModeSettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            bool fullScreen = Screen.fullScreen;

            if (carousel.windowModeSettingsCarousel.FullScreen != fullScreen) {
                ScheduleEvent(new SetCarouselItemIndexEvent(graphicsSettingsIndexes.component.CurrentWindowModeIndex), carousel);
            }

            carousel.windowModeSettingsCarousel.FullScreen = fullScreen;
        }

        [OnEventFire]
        public void ChangeCarouselItem(GraphicsSettingsChangedEvent e, GraphicsSettingsScreenNode screen, [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel,
            [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel, [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel,
            [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel, [JoinByScreen] ReadyVegetationQualityCarouselNode vegetationQualitySettingsCarousel,
            [JoinByScreen] ReadyGrassQualityCarouselNode grassQualitySettingsCarousel, [JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualitySettingCarousel,
            [JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel, [JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel,
            [JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel, [JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel,
            [JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel, [JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel,
            [JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualityCarousel, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            UpdateRightMenuElements(qualityCarousel, screenResolutionCarousel, windowModeSettingsCarousel, saturationLevelSettingsCarousel, antialiasingQualitySettingCarousel,
                renderResolutionQualityCarousel, vegetationQualitySettingsCarousel, grassQualitySettingsCarousel, anisotropicQualitySettingCarousel, shadowQualitySettingCarousel,
                particleQualitySettingCarousel, textureQualitySettingCarousel, cartridgeAmountCarousel, vsyncQualityCarousel, screen, graphicsSettingsIndexes);
        }

        [OnEventFire]
        public void Cancel(ButtonClickEvent e, SingleNode<CancelButtonComponent> button, [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel, [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeCarousel, [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            [JoinByScreen] ReadyVegetationQualityCarouselNode vegetationQualitySettingsCarousel, [JoinByScreen] ReadyGrassQualityCarouselNode grassQualitySettingsCarousel,
            [JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualitySettingCarousel,
            [JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel, [JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel,
            [JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel, [JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel,
            [JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel, [JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel,
            [JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualityCarousel, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            ScheduleEvent(new SetCarouselItemIndexEvent(GraphicsSettings.INSTANCE.CurrentQualityLevel), qualityCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentScreenResolutionIndex), screenResolutionCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentWindowModeIndex), windowModeCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentSaturationLevelIndex), saturationLevelSettingsCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentAntialiasingQualityIndex), antialiasingQualitySettingCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentRenderResolutionQualityIndex), renderResolutionQualityCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentVegetationQualityIndex), vegetationQualitySettingsCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentGrassQualityIndex), grassQualitySettingsCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentAnisotropicQualityIndex), anisotropicQualitySettingCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentShadowQualityIndex), shadowQualitySettingCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentParticleQualityIndex), particleQualitySettingCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentTextureQualityIndex), textureQualitySettingCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentCartridgeCaseAmountIndex), cartridgeAmountCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentVSyncQualityIndex), vsyncQualityCarousel);
        }

        [OnEventFire]
        public void SetDefault(ButtonClickEvent e, SingleNode<DefaultButtonComponent> button, [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel, [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeCarousel, [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            [JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualityCarousel, [JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel,
            [JoinByScreen] ReadyVegetationQualityCarouselNode vegetationQualityCarousel, [JoinByScreen] ReadyGrassQualityCarouselNode grassQualityCarousel,
            [JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel, [JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel,
            [JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel, [JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel,
            [JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel, [JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualityCarousel,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            ScheduleEvent(new SetCarouselItemIndexEvent(GraphicsSettings.INSTANCE.DefaultQualityLevel), qualityCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultScreenResolutionIndex), screenResolutionCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultWindowModeIndex), windowModeCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultSaturationLevelIndex), saturationLevelSettingsCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultVegetationQualityIndex), vegetationQualityCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultGrassQualityIndex), grassQualityCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultAntialiasingQualityIndex), antialiasingQualityCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultRenderResolutionQualityIndex), renderResolutionQualityCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultAnisotropicQualityIndex), anisotropicQualitySettingCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultShadowQualityIndex), shadowQualitySettingCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultParticleQualityIndex), particleQualitySettingCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultTextureQualityIndex), textureQualitySettingCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultCartridgeCaseAmountIndex), cartridgeAmountCarousel);
            ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultVSyncQualityIndex), vsyncQualityCarousel);
        }

        [OnEventFire]
        public void Apply(ButtonClickEvent e, SingleNode<ApplyButtonComponent> button, [JoinByScreen] GraphicsSettingsScreenNode screen,
            [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel, [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            [JoinByScreen] ReadyVegetationQualityCarouselNode vegetationQualityCarousel, [JoinByScreen] ReadyGrassQualityCarouselNode grassQualityCarousel,
            [JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualityCarousel, [JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel,
            [JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel, [JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel,
            [JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel, [JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel,
            [JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel, [JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualityCarousel,
            [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel, [JoinByCarousel] CurrentWindowModeCarouselItemNode windowMode,
            SingleNode<ApplyButtonComponent> button1, [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            [JoinByCarousel] CurrentScreenResolutionCarouselItemNode screenResolutionItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings iNSTANCE = GraphicsSettings.INSTANCE;
            bool windowed = windowMode.windowModeVariant.Windowed;
            iNSTANCE.ApplyWindowMode(windowed);
            windowModeSettingsCarousel.windowModeSettingsCarousel.FullScreen = !windowed;
            ScreenResolutionVariantComponent screenResolutionVariant = screenResolutionItem.screenResolutionVariant;
            int resolutionWidth = screenResolutionVariant.Width;
            int resolutionHeight = screenResolutionVariant.Height;
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            component.CurrentScreenResolutionIndex = iNSTANCE.ScreenResolutions.FindIndex(r => r.width == resolutionWidth && r.height == resolutionHeight);
            iNSTANCE.ApplyScreenResolution(resolutionWidth, resolutionHeight, windowed);
            graphicsSettingsIndexes.component.CurrentSaturationLevelIndex = saturationLevelSettingsCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentVegetationQualityIndex = vegetationQualityCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentGrassQualityIndex = grassQualityCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentAntialiasingQualityIndex = antialiasingQualityCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentRenderResolutionQualityIndex = renderResolutionQualityCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentAnisotropicQualityIndex = anisotropicQualitySettingCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentShadowQualityIndex = shadowQualitySettingCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentParticleQualityIndex = particleQualitySettingCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentTextureQualityIndex = textureQualitySettingCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentCartridgeCaseAmountIndex = cartridgeAmountCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentVSyncQualityIndex = vsyncQualityCarousel.carouselCurrentItemIndex.Index;

            if (!screen.graphicsSettingsScreen.NeedToReloadApplication) {
                ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
                return;
            }

            int index = qualityCarousel.carouselCurrentItemIndex.Index;
            iNSTANCE.ApplyQualityLevel(index);
            ScheduleEvent<SwitchToEntranceSceneEvent>(button);
        }

        void UpdateRightMenuElements(ReadyQualitySettingsCarouselNode qualityCarousel, ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel,
            ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel, ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel,
            ReadyAntialiasingQualityCarouselNode antialiasingQualitySettingCarousel, ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel,
            ReadyVegetationQualityCarouselNode vegetationQualitySettingCarousel, ReadyGrassQualityCarouselNode grassQualitySettingCarousel,
            ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel, ReadyShadowQualityCarouselNode shadowQualitySettingCarousel,
            ReadyParticleQualityCarouselNode particleQualitySettingCarousel, ReadyTextureQualityCarouselNode textureQualitySettingCarousel,
            ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel, ReadyVSyncQualityCarouselNode vsyncQualityCarousel, GraphicsSettingsScreenNode screen,
            SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettingsScreenComponent graphicsSettingsScreen = screen.graphicsSettingsScreen;
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            int index = qualityCarousel.carouselCurrentItemIndex.Index;
            int index2 = screenResolutionCarousel.carouselCurrentItemIndex.Index;
            int index3 = windowModeSettingsCarousel.carouselCurrentItemIndex.Index;
            int index4 = saturationLevelSettingsCarousel.carouselCurrentItemIndex.Index;
            int index5 = vegetationQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int index6 = grassQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int index7 = antialiasingQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int index8 = renderResolutionQualityCarousel.carouselCurrentItemIndex.Index;
            int index9 = anisotropicQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int index10 = shadowQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int index11 = particleQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int index12 = textureQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int index13 = cartridgeAmountCarousel.carouselCurrentItemIndex.Index;
            int index14 = vsyncQualityCarousel.carouselCurrentItemIndex.Index;
            bool needToShowChangePerfomance = index > GraphicsSettings.INSTANCE.DefaultQualityLevel;
            bool isCurrentQuality = index == GraphicsSettings.INSTANCE.CurrentQualityLevel;

            bool flag = index != GraphicsSettings.INSTANCE.CurrentQualityLevel || index5 != component.CurrentVegetationQualityIndex || index6 != component.CurrentGrassQualityIndex ||
                        index7 != component.CurrentAntialiasingQualityIndex || index8 != component.CurrentRenderResolutionQualityIndex ||
                        index9 != component.CurrentAnisotropicQualityIndex || index10 != component.CurrentShadowQualityIndex || index11 != component.CurrentParticleQualityIndex ||
                        index12 != component.CurrentTextureQualityIndex || index14 != component.CurrentVSyncQualityIndex;

            bool needToShowButtons = index3 != component.CurrentWindowModeIndex || index2 != component.CurrentScreenResolutionIndex || flag;

            bool defaultButtonVisibility = index != GraphicsSettings.INSTANCE.DefaultQualityLevel || index2 != component.DefaultScreenResolutionIndex ||
                                           index3 != component.DefaultWindowModeIndex || index4 != component.DefaultSaturationLevelIndex ||
                                           index5 != component.DefaultVegetationQualityIndex || index6 != component.DefaultGrassQualityIndex ||
                                           index7 != component.DefaultAntialiasingQualityIndex || index8 != component.DefaultRenderResolutionQualityIndex ||
                                           index9 != component.DefaultAnisotropicQualityIndex || index10 != component.DefaultShadowQualityIndex ||
                                           index11 != component.DefaultParticleQualityIndex || index12 != component.DefaultTextureQualityIndex ||
                                           index14 != component.DefaultVSyncQualityIndex;

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

        public class CustomSettingsCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public CustomSettingsCheckboxComponent customSettingsCheckbox;
        }

        public class AmbientOcclusionCheckboxNode : Node {
            public AmbientOcclusionCheckboxComponent ambientOcclusionCheckbox;
            public CheckboxComponent checkbox;

            public DependentInteractivityComponent dependentInteractivity;
        }

        public class BloomCheckboxNode : Node {
            public BloomCheckboxComponent bloomCheckbox;
            public CheckboxComponent checkbox;

            public DependentInteractivityComponent dependentInteractivity;
        }

        public class ChromaticAberrationCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public ChromaticAberrationCheckboxComponent chromaticAberrationCheckbox;

            public DependentInteractivityComponent dependentInteractivity;
        }

        public class GrainCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public DependentInteractivityComponent dependentInteractivity;

            public GrainCheckboxComponent grainCheckbox;
        }

        public class VignetteCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public DependentInteractivityComponent dependentInteractivity;

            public VignetteCheckboxComponent vignetteCheckbox;
        }

        public class DisableBattleNotificationsCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public DependentInteractivityComponent dependentInteractivity;

            public DisableBattleNotificationsCheckboxComponent disableBattleNotificationsCheckbox;
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

        public class VegetationQualityCarouselNode : CarouselNode {
            public DependentInteractivityComponent dependentInteractivity;
            public VegetationQualityCarouselComponent vegetationQualityCarousel;
        }

        public class GrassQualityCarouselNode : CarouselNode {
            public DependentInteractivityComponent dependentInteractivity;
            public GrassQualityCarouselComponent grassQualityCarousel;
        }

        public class RenderResolutionQualityCarouselNode : CarouselNode {
            public DependentInteractivityComponent dependentInteractivity;
            public RenderResolutionQualityCarouselComponent renderResolutionQualityCarousel;
        }

        public class AntialiasingQualityCarouselNode : CarouselNode {
            public AntialiasingQualityCarouselComponent antialiasingQualityCarousel;

            public DependentInteractivityComponent dependentInteractivity;
        }

        public class AnisotropicQualityCarouselNode : CarouselNode {
            public AnisotropicQualityCarouselComponent anisotropicQualityCarousel;

            public DependentInteractivityComponent dependentInteractivity;
        }

        public class ShadowQualityCarouselNode : CarouselNode {
            public DependentInteractivityComponent dependentInteractivity;
            public ShadowQualityCarouselComponent shadowQualityCarousel;
        }

        public class ParticleQualityCarouselNode : CarouselNode {
            public DependentInteractivityComponent dependentInteractivity;
            public ParticleQualityCarouselComponent particleQualityCarousel;
        }

        public class TextureQualityCarouselNode : CarouselNode {
            public DependentInteractivityComponent dependentInteractivity;
            public TextureQualityCarouselComponent textureQualityCarousel;
        }

        public class CartridgeCaseAmountCarouselNode : CarouselNode {
            public CartridgeCaseAmountCarouselComponent cartridgeCaseAmountCarousel;

            public DependentInteractivityComponent dependentInteractivity;
        }

        public class VSyncQualityCarouselNode : CarouselNode {
            public DependentInteractivityComponent dependentInteractivity;
            public VSyncQualityCarouselComponent vSyncQualityCarousel;
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

        public class ReadyVegetationQualityCarouselNode : ReadyCarouselNode {
            public VegetationQualityCarouselComponent vegetationQualityCarousel;
        }

        public class ReadyGrassQualityCarouselNode : ReadyCarouselNode {
            public GrassQualityCarouselComponent grassQualityCarousel;
        }

        public class ReadyRenderResolutionQualityCarouselNode : ReadyCarouselNode {
            public RenderResolutionQualityCarouselComponent renderResolutionQualityCarousel;
        }

        public class ReadyAntialiasingQualityCarouselNode : ReadyCarouselNode {
            public AntialiasingQualityCarouselComponent antialiasingQualityCarousel;
        }

        public class ReadyAnisotropicQualityCarouselNode : ReadyCarouselNode {
            public AnisotropicQualityCarouselComponent anisotropicQualityCarousel;
        }

        public class ReadyTextureQualityCarouselNode : ReadyCarouselNode {
            public TextureQualityCarouselComponent textureQualityCarousel;
        }

        public class ReadyShadowQualityCarouselNode : ReadyCarouselNode {
            public ShadowQualityCarouselComponent shadowQualityCarousel;
        }

        public class ReadyParticleQualityCarouselNode : ReadyCarouselNode {
            public ParticleQualityCarouselComponent particleQualityCarousel;
        }

        public class ReadyCartridgeAmountCarouselNode : ReadyCarouselNode {
            public CartridgeCaseAmountCarouselComponent cartridgeCaseAmountCarousel;
        }

        public class ReadyVSyncQualityCarouselNode : ReadyCarouselNode {
            public VSyncQualityCarouselComponent vSyncQualityCarousel;
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

        public class CurrentVegetationQualityCarouselItemNode : CurrentCarouselItemNode {
            public VegetationSettingsComponent vegetationSettings;
        }

        public class CurrentGrassQualityCarouselItemNode : CurrentCarouselItemNode {
            public GrassSettingsComponent grassSettings;
        }

        public class CurrentRenderResolutionQualityCarouselItemNode : CurrentCarouselItemNode {
            public RenderResolutionQualityVariantComponent renderResolutionQualityVariant;
        }

        public class CurrentAntialiasingQualityCarouselItemNode : CurrentCarouselItemNode {
            public AntialiasingQualityVariantComponent antialiasingQualityVariant;
        }

        public class CurrentAnisotropicQualityCarouselItemNode : CurrentCarouselItemNode {
            public AnisotropicQualityVariantComponent anisotropicQualityVariant;
        }

        public class CurrentTextureQualityCarouselItemNode : CurrentCarouselItemNode {
            public TextureQualityVariantComponent textureQualityVariant;
        }

        public class CurrentShadowQualityCarouselItemNode : CurrentCarouselItemNode {
            public ShadowQualityVariantComponent shadowQualityVariant;
        }

        public class CurrentParticleQualityCarouselItemNode : CurrentCarouselItemNode {
            public ParticleQualityVariantComponent particleQualityVariant;
        }

        public class CurrentCartridgeCaseAmountCarouselItemNode : CurrentCarouselItemNode {
            public CartridgeCaseSettingVariantComponent cartridgeCaseSettingVariant;
        }

        public class CurrentVSyncCarouselItemNode : CurrentCarouselItemNode {
            public VSyncSettingVariantComponent vSyncSettingVariant;
        }
    }
}