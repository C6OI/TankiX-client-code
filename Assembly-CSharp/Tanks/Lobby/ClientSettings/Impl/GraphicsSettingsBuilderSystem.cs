using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.Impl {
    public class GraphicsSettingsBuilderSystem : ECSSystem {
        int currentAnisotropicQualityIndex;

        int currentAntialiasingQualityIndex;

        int currentCartridgeCaseAmountIndex;

        int currentGrassLevelIndex;

        int currentParticleQualityIndex;

        int currentRenderResolutionQualityIndex;

        int currentShadowQualityIndex;

        int currentTextureQualityIndex;

        int currentVegetationLevelIndex;

        int currentVSyncQualityIndex;

        int defaultAnisotropicQualityIndex;

        int defaultAntialiasingQualityIndex;

        int defaultCartridgeCaseAmountIndex;

        int defaultGrassLevelIndex;

        int defaultParticleQualityIndex;

        int defaultRenderResolutionQualityIndex;

        int defaultShadowQualityIndex;

        int defaultTextureQualityIndex;

        int defaultVegetationLevelIndex;

        int defaultVSyncQualityIndex;

        [Inject] public new static ConfigurationService ConfigurationService { get; set; }

        [OnEventFire]
        public void SetPostProcessing(NodeAddedEvent e, SingleNode<PostProcessingQualityVariantComponent> settings,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            GraphicsSettings.INSTANCE.customSettings = settings.component.CustomSettings;
            GraphicsSettings.INSTANCE.currentAmbientOcclusion = settings.component.AmbientOcclusion;
            GraphicsSettings.INSTANCE.currentBloom = settings.component.Bloom;
            GraphicsSettings.INSTANCE.currentChromaticAberration = settings.component.ChromaticAberration;
            GraphicsSettings.INSTANCE.currentGrain = settings.component.Grain;
            GraphicsSettings.INSTANCE.currentVignette = settings.component.Vignette;

            if (!GraphicsSettings.INSTANCE.customSettings) {
                GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
                BuildDefaultSettings();
                component.CurrentAnisotropicQualityIndex = currentAnisotropicQualityIndex;
                component.DefaultAnisotropicQualityIndex = defaultAnisotropicQualityIndex;
                component.CurrentAntialiasingQualityIndex = currentAntialiasingQualityIndex;
                component.DefaultAntialiasingQualityIndex = defaultAntialiasingQualityIndex;
                component.CurrentRenderResolutionQualityIndex = currentRenderResolutionQualityIndex;
                component.DefaultRenderResolutionQualityIndex = defaultRenderResolutionQualityIndex;
                component.CurrentGrassQualityIndex = currentGrassLevelIndex;
                component.DefaultGrassQualityIndex = defaultGrassLevelIndex;
                component.CurrentShadowQualityIndex = currentShadowQualityIndex;
                component.DefaultShadowQualityIndex = defaultShadowQualityIndex;
                component.CurrentParticleQualityIndex = currentParticleQualityIndex;
                component.DefaultParticleQualityIndex = defaultParticleQualityIndex;
                component.CurrentTextureQualityIndex = currentTextureQualityIndex;
                component.DefaultTextureQualityIndex = defaultTextureQualityIndex;
                component.CurrentVegetationQualityIndex = currentVegetationLevelIndex;
                component.DefaultVegetationQualityIndex = defaultVegetationLevelIndex;
                component.CurrentCartridgeCaseAmountIndex = currentCartridgeCaseAmountIndex;
                component.DefaultCartridgeCaseAmountIndex = defaultCartridgeCaseAmountIndex;
                component.CurrentVSyncQualityIndex = currentVSyncQualityIndex;
                component.DefaultVSyncQualityIndex = defaultVSyncQualityIndex;
            }
        }

        [OnEventFire]
        public void BuildSaturationGraphicsSettings(NodeAddedEvent e, SaturationLevelSettingsBuilderNode builder) {
            BuildGraphicsSettings<SaturationLevelVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildVegetationGraphicsSettings(NodeAddedEvent e, VegetationSettingsBuilderNode builder) {
            BuildGraphicsSettings<VegetationSettingsTemplate>(builder);
        }

        [OnEventFire]
        public void BuildGrassGraphicsSettings(NodeAddedEvent e, GrassSettingsBuilderNode builder) {
            BuildGraphicsSettings<GrassSettingsTemplate>(builder);
        }

        [OnEventFire]
        public void BuildAnisotropicGraphicsSettings(NodeAddedEvent e, AnisotropicQualitySettingsBuilderNode builder) {
            BuildGraphicsSettings<AnisotropicQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildRenderResolutionGraphicsSettings(NodeAddedEvent e, RenderResolutionQualitySettingsBuilderNode builder) {
            BuildGraphicsSettings<RenderResolutionQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildCartridgeCaseAmountSettings(NodeAddedEvent e, CartridgeCaseAmountSettingsBuilderNode builder) {
            BuildGraphicsSettings<CartridgeCaseSettingVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildVSyncQualitySettings(NodeAddedEvent e, VSyncQualitySettingsBuilderNode builder) {
            BuildGraphicsSettings<VSyncSettingVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildAntialiasingGraphicsSettings(NodeAddedEvent e, AntialiasingQualitySettingsBuilderNode builder) {
            BuildGraphicsSettings<AntialiasingQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildShadowGraphicsSettings(NodeAddedEvent e, ShadowQualitySettingsBuilderNode builder) {
            BuildGraphicsSettings<ShadowQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildParticleGraphicsSettings(NodeAddedEvent e, ParticleQualitySettingsBuilderNode builder) {
            BuildGraphicsSettings<ParticleQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildTextureGraphicsSettings(NodeAddedEvent e, TextureQualitySettingsBuilderNode builder) {
            BuildGraphicsSettings<TextureQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildSaturationLevelSettings(GraphicsSettingsBuilderReadyEvent e, SaturationLevelSettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<SaturationLevelVariantComponent>> saturationLevels, SaturationLevelSettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentSaturationLevel = GraphicsSettings.INSTANCE.CurrentSaturationLevel;
            SingleNode<SaturationLevelVariantComponent> singleNode = saturationLevels.OrderBy(i => Mathf.Abs(i.component.Value - currentSaturationLevel)).First();

            SingleNode<SaturationLevelVariantComponent> singleNode2 =
                saturationLevels.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultSaturationLevel)).First();

            GraphicsSettings.INSTANCE.ApplySaturationLevel(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentSaturationLevelIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultSaturationLevelIndex = items.IndexOf(singleNode2.Entity);
        }

        [OnEventFire]
        public void BuildVegetationQualitySettings(GraphicsSettingsBuilderReadyEvent e, VegetationSettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<VegetationSettingsComponent>> vegetationSettings, VegetationSettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentVegetationLevel = GraphicsSettings.INSTANCE.CurrentVegetationLevel;
            SingleNode<VegetationSettingsComponent> singleNode = vegetationSettings.OrderBy(i => Mathf.Abs(i.component.Value - currentVegetationLevel)).First();
            SingleNode<VegetationSettingsComponent> singleNode2 = vegetationSettings.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultVegetationLevel)).First();
            GraphicsSettings.INSTANCE.ApplyVegetationLevel(singleNode.component.Value, singleNode2.component.Value);
            GraphicsSettings.INSTANCE.ApplyFarFoliageEnabled(singleNode.component.FarFoliageEnabled, singleNode2.component.FarFoliageEnabled);
            GraphicsSettings.INSTANCE.ApplyTreesShadowRecieving(singleNode.component.BillboardTreesShadowReceiving, singleNode2.component.BillboardTreesShadowReceiving);
            GraphicsSettings.INSTANCE.ApplyBillboardTreesShadowCasting(singleNode.component.BillboardTreesShadowCasting, singleNode2.component.BillboardTreesShadowCasting);
            graphicsSettingsIndexes.component.CurrentVegetationQualityIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultVegetationQualityIndex = items.IndexOf(singleNode2.Entity);
            defaultVegetationLevelIndex = graphicsSettingsIndexes.component.DefaultVegetationQualityIndex;
        }

        [OnEventFire]
        public void BuildGrassQualitySettings(GraphicsSettingsBuilderReadyEvent e, GrassSettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<GrassSettingsComponent>> grassSettings, GrassSettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentGrassLevel = GraphicsSettings.INSTANCE.CurrentGrassLevel;
            SingleNode<GrassSettingsComponent> singleNode = grassSettings.OrderBy(i => Mathf.Abs(i.component.Value - currentGrassLevel)).First();
            SingleNode<GrassSettingsComponent> singleNode2 = grassSettings.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultGrassLevel)).First();
            GraphicsSettings.INSTANCE.ApplyGrassLevel(singleNode.component.Value, singleNode2.component.Value);
            GraphicsSettings.INSTANCE.ApplyGrassFarDrawDistance(singleNode.component.GrassFarDrawDistance, singleNode2.component.GrassFarDrawDistance);
            GraphicsSettings.INSTANCE.ApplyGrassNearDrawDistance(singleNode.component.GrassNearDrawDistance, singleNode2.component.GrassNearDrawDistance);
            GraphicsSettings.INSTANCE.ApplyGrassFadeRange(singleNode.component.GrassFadeRange, singleNode2.component.GrassFadeRange);
            GraphicsSettings.INSTANCE.ApplyGrassDensityMultiplier(singleNode.component.GrassDensityMultiplier, singleNode2.component.GrassDensityMultiplier);
            GraphicsSettings.INSTANCE.ApplyGrassCastsShadow(singleNode.component.GrassCastsShadow, singleNode2.component.GrassCastsShadow);
            graphicsSettingsIndexes.component.CurrentGrassQualityIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultGrassQualityIndex = items.IndexOf(singleNode2.Entity);
            defaultGrassLevelIndex = graphicsSettingsIndexes.component.DefaultGrassQualityIndex;
        }

        [OnEventFire]
        public void BuildAnisotropicQualitySettings(GraphicsSettingsBuilderReadyEvent e, AnisotropicQualitySettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<AnisotropicQualityVariantComponent>> anisotropicQuality, AnisotropicQualitySettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentAnisotropicQuality = GraphicsSettings.INSTANCE.CurrentAnisotropicQuality;
            SingleNode<AnisotropicQualityVariantComponent> singleNode = anisotropicQuality.OrderBy(i => Mathf.Abs(i.component.Value - currentAnisotropicQuality)).First();

            SingleNode<AnisotropicQualityVariantComponent> singleNode2 =
                anisotropicQuality.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultAnisotropicQuality)).First();

            GraphicsSettings.INSTANCE.ApplyAnisotropicQuality(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentAnisotropicQualityIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultAnisotropicQualityIndex = items.IndexOf(singleNode2.Entity);
            defaultAnisotropicQualityIndex = graphicsSettingsIndexes.component.DefaultAnisotropicQualityIndex;
            AnisotropicFiltering anisotropicFiltering = (AnisotropicFiltering)singleNode.component.AnisotropicFiltering;
            QualitySettings.anisotropicFiltering = anisotropicFiltering;
        }

        [OnEventFire]
        public void BuildRenderResolutionQualitySettings(GraphicsSettingsBuilderReadyEvent e, RenderResolutionQualitySettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<RenderResolutionQualityVariantComponent>> renderResolutionQuality, RenderResolutionQualitySettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentRenderResulutionQuality = GraphicsSettings.INSTANCE.CurrentRenderResolutionQuality;
            SingleNode<RenderResolutionQualityVariantComponent> singleNode = renderResolutionQuality.OrderBy(i => Mathf.Abs(i.component.Value - currentRenderResulutionQuality)).First();

            SingleNode<RenderResolutionQualityVariantComponent> singleNode2 =
                renderResolutionQuality.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultRenderResolutionQuality)).First();

            GraphicsSettings.INSTANCE.ApplyRenderResolutionQuality(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentRenderResolutionQualityIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultRenderResolutionQualityIndex = items.IndexOf(singleNode2.Entity);
            defaultRenderResolutionQualityIndex = graphicsSettingsIndexes.component.DefaultRenderResolutionQualityIndex;
        }

        [OnEventFire]
        public void BuildAntialiasingQualitySettings(GraphicsSettingsBuilderReadyEvent e, AntialiasingQualitySettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<AntialiasingQualityVariantComponent>> antialiasingQuality, AntialiasingQualitySettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentAntialiasingQuality = GraphicsSettings.INSTANCE.CurrentAntialiasingQuality;
            SingleNode<AntialiasingQualityVariantComponent> singleNode = antialiasingQuality.OrderBy(i => Mathf.Abs(i.component.Value - currentAntialiasingQuality)).First();

            SingleNode<AntialiasingQualityVariantComponent> singleNode2 =
                antialiasingQuality.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultAntialiasingQuality)).First();

            GraphicsSettings.INSTANCE.ApplyAntialiasingQuality(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentAntialiasingQualityIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultAntialiasingQualityIndex = items.IndexOf(singleNode2.Entity);
            defaultAntialiasingQualityIndex = graphicsSettingsIndexes.component.DefaultAntialiasingQualityIndex;
        }

        [OnEventFire]
        public void BuildShadowQualitySettings(GraphicsSettingsBuilderReadyEvent e, ShadowQualitySettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<ShadowQualityVariantComponent>> shadowQuality, ShadowQualitySettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentShadowQuality = GraphicsSettings.INSTANCE.CurrentShadowQuality;
            SingleNode<ShadowQualityVariantComponent> singleNode = shadowQuality.OrderBy(i => Mathf.Abs(i.component.Value - currentShadowQuality)).First();
            SingleNode<ShadowQualityVariantComponent> singleNode2 = shadowQuality.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultShadowQuality)).First();
            GraphicsSettings.INSTANCE.ApplyShadowQuality(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentShadowQualityIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultShadowQualityIndex = items.IndexOf(singleNode2.Entity);
            defaultShadowQualityIndex = graphicsSettingsIndexes.component.DefaultShadowQualityIndex;
            ShadowQuality shadowQuality2 = (ShadowQuality)singleNode.component.ShadowQuality;
            ShadowResolution shadowResolution = (ShadowResolution)singleNode.component.ShadowResolution;
            ShadowProjection shadowProjection = (ShadowProjection)singleNode.component.ShadowProjection;
            QualitySettings.shadows = shadowQuality2;
            QualitySettings.shadowResolution = shadowResolution;
            QualitySettings.shadowProjection = shadowProjection;
            QualitySettings.shadowDistance = singleNode.component.ShadowDistance;
            QualitySettings.shadowNearPlaneOffset = singleNode.component.ShadowNearPlaneOffset;
            QualitySettings.shadowCascades = singleNode.component.ShadowCascades;
        }

        [OnEventFire]
        public void BuildParticleQualitySettings(GraphicsSettingsBuilderReadyEvent e, ParticleQualitySettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<ParticleQualityVariantComponent>> particleQuality, ParticleQualitySettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentParticleQuality = GraphicsSettings.INSTANCE.CurrentParticleQuality;
            SingleNode<ParticleQualityVariantComponent> singleNode = particleQuality.OrderBy(i => Mathf.Abs(i.component.Value - currentParticleQuality)).First();

            SingleNode<ParticleQualityVariantComponent> singleNode2 =
                particleQuality.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultParticleQuality)).First();

            GraphicsSettings.INSTANCE.ApplyParticleQuality(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentParticleQualityIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultParticleQualityIndex = items.IndexOf(singleNode2.Entity);
            defaultParticleQualityIndex = graphicsSettingsIndexes.component.DefaultParticleQualityIndex;
        }

        [OnEventFire]
        public void BuildTextureQualitySettings(GraphicsSettingsBuilderReadyEvent e, TextureQualitySettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<TextureQualityVariantComponent>> textureQuality, TextureQualitySettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            float currentTextureQuality = GraphicsSettings.INSTANCE.CurrentTextureQuality;
            SingleNode<TextureQualityVariantComponent> singleNode = textureQuality.OrderBy(i => Mathf.Abs(i.component.Value - currentTextureQuality)).First();
            SingleNode<TextureQualityVariantComponent> singleNode2 = textureQuality.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultTextureQuality)).First();
            GraphicsSettings.INSTANCE.ApplyTextureQuality(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentTextureQualityIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultTextureQualityIndex = items.IndexOf(singleNode2.Entity);
            defaultTextureQualityIndex = graphicsSettingsIndexes.component.DefaultTextureQualityIndex;
            QualitySettings.masterTextureLimit = singleNode.component.MasterTextureLimit;
        }

        [OnEventFire]
        public void BuildCartridgeCaseAmountSettings(GraphicsSettingsBuilderReadyEvent e, CartridgeCaseAmountSettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<CartridgeCaseSettingVariantComponent>> cartridgeAmount, CartridgeCaseAmountSettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            int currentCartridgeCaseAmount = GraphicsSettings.INSTANCE.CurrentCartridgeCaseAmount;
            SingleNode<CartridgeCaseSettingVariantComponent> singleNode = cartridgeAmount.OrderBy(i => Mathf.Abs(i.component.Value - currentCartridgeCaseAmount)).First();

            SingleNode<CartridgeCaseSettingVariantComponent> singleNode2 =
                cartridgeAmount.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultRenderResolutionQuality)).First();

            GraphicsSettings.INSTANCE.ApplyCartridgeCaseAmount(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentCartridgeCaseAmountIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultCartridgeCaseAmountIndex = items.IndexOf(singleNode2.Entity);
            defaultCartridgeCaseAmountIndex = graphicsSettingsIndexes.component.DefaultCartridgeCaseAmountIndex;
        }

        [OnEventFire]
        public void BuildVSyncQualitySettings(GraphicsSettingsBuilderReadyEvent e, VSyncQualitySettingsBuilderNode builder,
            [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<VSyncSettingVariantComponent>> vSync, VSyncQualitySettingsBuilderNode builder1,
            [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            int currentVSyncQuality = GraphicsSettings.INSTANCE.CurrentVSyncQuality;
            SingleNode<VSyncSettingVariantComponent> singleNode = vSync.OrderBy(i => Mathf.Abs(i.component.Value - currentVSyncQuality)).First();
            SingleNode<VSyncSettingVariantComponent> singleNode2 = vSync.OrderBy(i => Mathf.Abs(i.component.Value - GraphicsSettings.INSTANCE.DefaultRenderResolutionQuality)).First();
            GraphicsSettings.INSTANCE.ApplyVSyncQuality(singleNode.component.Value, singleNode2.component.Value);
            graphicsSettingsIndexes.component.CurrentVSyncQualityIndex = items.IndexOf(singleNode.Entity);
            graphicsSettingsIndexes.component.DefaultVSyncQualityIndex = items.IndexOf(singleNode2.Entity);
            defaultVSyncQualityIndex = graphicsSettingsIndexes.component.DefaultVSyncQualityIndex;
            QualitySettings.vSyncCount = singleNode.component.Value;
        }

        [OnEventFire]
        public void BuildScreenResolutionSettings(NodeAddedEvent evt, SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            graphicsSettingsIndexes.component.CalculateScreenResolutionIndexes();
        }

        [OnEventComplete]
        public void CleanBuilder(GraphicsSettingsBuilderReadyEvent e, GraphicsSettingsBuilderNode builder) {
            builder.graphicsSettingsBuilder.Items.ForEach(delegate(Entity item) {
                DeleteEntity(item);
            });

            DeleteEntity(builder.Entity);
        }

        void BuildGraphicsSettings<T>(GraphicsSettingsBuilderNode builder) where T : Template {
            GraphicsSettingsBuilderGroupComponent group = builder.Entity.CreateGroup<GraphicsSettingsBuilderGroupComponent>();
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
        public void InitWindowModeSettingsCarouselItems(NodeAddedEvent e, WindowModeSettingCarouselNode windowModeSettingCarousel,
            SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes) {
            Entity item = CreateWindowModeItem(windowModeSettingCarousel.windowModes.Fullscreen, false, windowModeSettingCarousel.carouselGroup);
            Entity item2 = CreateWindowModeItem(windowModeSettingCarousel.windowModes.Windowed, true, windowModeSettingCarousel.carouselGroup);
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

        public void BuildDefaultSettings() {
            if (QualitySettings.GetQualityLevel() == 0) {
                BuildSettings(ShadowQuality.Disable, ShadowResolution.Low, ShadowProjection.StableFit, 0f, 2f, 0, 1, AnisotropicFiltering.Disable, 0, 0, 0, 0, 0, 0, 0, true, false, false,
                    0, 0f, 0f, 15f, 0f, false, 1, 0, 0, 1);
            }

            if (QualitySettings.GetQualityLevel() == 1) {
                BuildSettings(ShadowQuality.Disable, ShadowResolution.Low, ShadowProjection.StableFit, 0f, 2f, 0, 1, AnisotropicFiltering.Disable, 0, 0, 0, 0, 0, 0, 0, true, false, false,
                    0, 0f, 0f, 15f, 0f, false, 0, 1, 0, 1);
            }

            if (QualitySettings.GetQualityLevel() == 2) {
                BuildSettings(ShadowQuality.All, ShadowResolution.Medium, ShadowProjection.StableFit, 70f, 2f, 0, 0, AnisotropicFiltering.Disable, 1, 1, 0, 1, 1, 0, 1, true, true, true,
                    1, 0f, 0f, 15f, 0f, false, 0, 2, 0, 1);
            }

            if (QualitySettings.GetQualityLevel() == 3) {
                BuildSettings(ShadowQuality.All, ShadowResolution.High, ShadowProjection.StableFit, 100f, 2f, 0, 0, AnisotropicFiltering.Enable, 2, 2, 1, 1, 2, 0, 2, true, true, true, 2,
                    80f, 65f, 15f, 0.4f, false, 0, 3, 1, 1);
            }

            if (QualitySettings.GetQualityLevel() == 4) {
                BuildSettings(ShadowQuality.All, ShadowResolution.High, ShadowProjection.StableFit, 100f, 2f, 2, 0, AnisotropicFiltering.ForceEnable, 3, 3, 2, 1, 3, 1, 3, true, true,
                    true, 3, 100f, 65f, 15f, 0.65f, true, 0, 4, 2, 1);
            }

            if (QualitySettings.GetQualityLevel() == 5) {
                BuildSettings(ShadowQuality.All, ShadowResolution.VeryHigh, ShadowProjection.StableFit, 500f, 2f, 4, 0, AnisotropicFiltering.ForceEnable, 4, 4, 2, 1, 4, 1, 4, true, true,
                    true, 4, 200f, 90f, 15f, 0.8f, true, 0, 5, 3, 1);
            }
        }

        public void BuildSettings(ShadowQuality shadowQuality, ShadowResolution shadowResolution, ShadowProjection shadowProjection, float shadowDistance, float shadowNearPlane,
            int shadowCascades, int masterTexture, AnisotropicFiltering anisotropicFiltering, int grassLevelIndex, int shadowQualityIndex, int anisotropicQualityIndex,
            int textureQualityIndex, int vegetationLevelIndex, int antialiasingQuality, int vegetationLevel, bool farFoliageEnabled, bool treesShadowRecieving,
            bool billboardTreesShadowCasting, int grassLevel, float grassFarDrawDistance, float grassNearDrawDistance, float grassFadeRange, float grassDensityMultiplier,
            bool grassCastsShadow, int renderResolutionQuality, int particleQuality, int cartridgeCaseAmount, int vsyncQuality) {
            QualitySettings.shadows = shadowQuality;
            QualitySettings.shadowResolution = shadowResolution;
            QualitySettings.shadowProjection = shadowProjection;
            QualitySettings.shadowDistance = shadowDistance;
            QualitySettings.shadowNearPlaneOffset = shadowNearPlane;
            QualitySettings.shadowCascades = shadowCascades;
            QualitySettings.masterTextureLimit = masterTexture;
            QualitySettings.anisotropicFiltering = anisotropicFiltering;
            QualitySettings.vSyncCount = vsyncQuality;
            currentGrassLevelIndex = grassLevelIndex;
            currentShadowQualityIndex = shadowQualityIndex;
            currentAnisotropicQualityIndex = anisotropicQualityIndex;
            currentTextureQualityIndex = textureQualityIndex;
            currentVegetationLevelIndex = vegetationLevelIndex;
            currentAntialiasingQualityIndex = antialiasingQuality;
            currentRenderResolutionQualityIndex = renderResolutionQuality;
            currentCartridgeCaseAmountIndex = cartridgeCaseAmount;
            currentVSyncQualityIndex = vsyncQuality;
            currentParticleQualityIndex = particleQuality;
            GraphicsSettings.INSTANCE.ApplyCartridgeCaseAmount(cartridgeCaseAmount, cartridgeCaseAmount);
            GraphicsSettings.INSTANCE.ApplyVSyncQuality(vsyncQuality, vsyncQuality);
            GraphicsSettings.INSTANCE.ApplyVegetationLevel(vegetationLevel, vegetationLevel);
            GraphicsSettings.INSTANCE.ApplyFarFoliageEnabled(farFoliageEnabled, farFoliageEnabled);
            GraphicsSettings.INSTANCE.ApplyTreesShadowRecieving(treesShadowRecieving, treesShadowRecieving);
            GraphicsSettings.INSTANCE.ApplyBillboardTreesShadowCasting(billboardTreesShadowCasting, billboardTreesShadowCasting);
            GraphicsSettings.INSTANCE.ApplyGrassLevel(grassLevel, grassLevel);
            GraphicsSettings.INSTANCE.ApplyGrassFarDrawDistance(grassFarDrawDistance, grassFarDrawDistance);
            GraphicsSettings.INSTANCE.ApplyGrassNearDrawDistance(grassNearDrawDistance, grassNearDrawDistance);
            GraphicsSettings.INSTANCE.ApplyGrassFadeRange(grassFadeRange, grassFadeRange);
            GraphicsSettings.INSTANCE.ApplyGrassDensityMultiplier(grassDensityMultiplier, grassDensityMultiplier);
            GraphicsSettings.INSTANCE.ApplyGrassCastsShadow(grassCastsShadow, grassCastsShadow);
        }

        public class GraphicsSettingsBuilderNode : Node {
            public ConfigPathCollectionComponent configPathCollection;
            public GraphicsSettingsBuilderComponent graphicsSettingsBuilder;
        }

        public class SaturationLevelSettingsBuilderNode : GraphicsSettingsBuilderNode {
            public SaturationLevelSettingsBuilderComponent saturationLevelSettingsBuilder;
        }

        public class VegetationSettingsBuilderNode : GraphicsSettingsBuilderNode {
            public VegetationSettingsBuilderComponent vegetationSettingsBuilder;
        }

        public class GrassSettingsBuilderNode : GraphicsSettingsBuilderNode {
            public GrassSettingsBuilderComponent grassSettingsBuilder;
        }

        public class AnisotropicQualitySettingsBuilderNode : GraphicsSettingsBuilderNode {
            public AnisotropicQualitySettingsBuilderComponent anisotropicQualitySettingsBuilder;
        }

        public class RenderResolutionQualitySettingsBuilderNode : GraphicsSettingsBuilderNode {
            public RenderResolutionQualitySettingsBuilderComponent renderResolutionQualitySettingsBuilder;
        }

        public class AntialiasingQualitySettingsBuilderNode : GraphicsSettingsBuilderNode {
            public AntialiasingQualitySettingsBuilderComponent antialiasingQualitySettingsBuilder;
        }

        public class ShadowQualitySettingsBuilderNode : GraphicsSettingsBuilderNode {
            public ShadowQualitySettingsBuilderComponent shadowQualitySettingsBuilder;
        }

        public class ParticleQualitySettingsBuilderNode : GraphicsSettingsBuilderNode {
            public ParticleQualitySettingsBuilderComponent particleQualitySettingsBuilder;
        }

        public class TextureQualitySettingsBuilderNode : GraphicsSettingsBuilderNode {
            public TextureQualitySettingsBuilderComponent textureQualitySettingsBuilder;
        }

        public class CartridgeCaseAmountSettingsBuilderNode : GraphicsSettingsBuilderNode {
            public CartridgeCaseAmountSettingBuilderComponent cartridgeCaseAmountSettingBuilder;
        }

        public class VSyncQualitySettingsBuilderNode : GraphicsSettingsBuilderNode {
            public VSyncSettingBuilderComponent vSyncSettingBuilder;
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