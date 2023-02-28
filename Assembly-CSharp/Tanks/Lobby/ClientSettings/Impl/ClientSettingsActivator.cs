using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.Impl {
    public class ClientSettingsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [SerializeField] string saturationLevelTemplatePath;

        [SerializeField] string antialiasingQualityTemplatePath;

        [SerializeField] string renderResolutionQualityTemplatePath;

        [SerializeField] string shadowQualityTemplatePath;

        [SerializeField] string particleQualityTemplatePath;

        [SerializeField] string textureQualityTemplatePath;

        [SerializeField] string anisotropicQualityTemplatePath;

        [SerializeField] string vegetationSettingsTemplatePath;

        [SerializeField] string grassSettingsTemplatePath;

        [SerializeField] string cartridgeCaseAmountTemplatePath;

        [SerializeField] string vsyncTemplatePath;

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new SelectLocaleScreenSystem());
            EngineService.RegisterSystem(new ScreenResolutionSettingsCarouselBuilderSystem());
            EngineService.RegisterSystem(new GraphicsSettingsScreenSystem());
            EngineService.RegisterSystem(new GraphicsSettingsBuilderSystem());
            EngineService.RegisterSystem(new PostProcessingQualitySystem());
            TemplateRegistry.Register<SettingsTemplate>();
            TemplateRegistry.Register<QualitySettingsVariantTemplate>();
            TemplateRegistry.Register<ScreenResolutionVariantTemplate>();
            TemplateRegistry.Register<WindowModesTemplate>();
            TemplateRegistry.Register<SaturationLevelVariantTemplate>();
            TemplateRegistry.Register<AnisotropicQualityVariantTemplate>();
            TemplateRegistry.Register<AntialiasingQualityVariantTemplate>();
            TemplateRegistry.Register<RenderResolutionQualityVariantTemplate>();
            TemplateRegistry.Register<ShadowQualityVariantTemplate>();
            TemplateRegistry.Register<ParticleQualityVariantTemplate>();
            TemplateRegistry.Register<TextureQualityVariantTemplate>();
            TemplateRegistry.Register<GraphicsSettingsBuilderTemplate>();
            TemplateRegistry.Register<SaturationLevelSettingsBuilderTemplate>();
            TemplateRegistry.Register<AnisotropicQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<AntialiasingQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<RenderResolutionQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<CartridgeCaseSettingVariantTemplate>();
            TemplateRegistry.Register<CartridgeCaseAmountSettingBuilderTemplate>();
            TemplateRegistry.Register<VSyncSettingVariantTemplate>();
            TemplateRegistry.Register<VSyncSettingBuilderTemplate>();
            TemplateRegistry.Register<ShadowQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<ParticleQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<TextureQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<GrassSettingsTemplate>();
            TemplateRegistry.Register<VegetationSettingsTemplate>();
            TemplateRegistry.Register<VegetationSettingsBuilderTemplate>();
            TemplateRegistry.Register<GrassSettingsBuilderTemplate>();
            TemplateRegistry.RegisterPart<CartridgeCaseSettingTemplatePart>();
            TemplateRegistry.RegisterPart<VSyncSettingTemplatePart>();
            TemplateRegistry.RegisterPart<PostProcessingQualityVariantTemplatePart>();
        }

        protected override void Activate() {
            Engine engine = EngineService.Engine;
            BuildGraphicsSettings(engine);
            engine.CreateEntity<SettingsTemplate>(string.Empty);
        }

        void BuildGraphicsSettings(Engine engine) {
            engine.CreateEntity("GraphicsSettingsIndexes").AddComponent<GraphicsSettingsIndexesComponent>();
            engine.CreateEntity<SaturationLevelSettingsBuilderTemplate>(saturationLevelTemplatePath);
            engine.CreateEntity<AnisotropicQualitySettingsBuilderTemplate>(anisotropicQualityTemplatePath);
            engine.CreateEntity<AntialiasingQualitySettingsBuilderTemplate>(antialiasingQualityTemplatePath);
            engine.CreateEntity<RenderResolutionQualitySettingsBuilderTemplate>(renderResolutionQualityTemplatePath);
            engine.CreateEntity<TextureQualitySettingsBuilderTemplate>(textureQualityTemplatePath);
            engine.CreateEntity<ShadowQualitySettingsBuilderTemplate>(shadowQualityTemplatePath);
            engine.CreateEntity<ParticleQualitySettingsBuilderTemplate>(particleQualityTemplatePath);
            engine.CreateEntity<VegetationSettingsBuilderTemplate>(vegetationSettingsTemplatePath);
            engine.CreateEntity<GrassSettingsBuilderTemplate>(grassSettingsTemplatePath);
            engine.CreateEntity<CartridgeCaseAmountSettingBuilderTemplate>(cartridgeCaseAmountTemplatePath);
            engine.CreateEntity<VSyncSettingBuilderTemplate>(vsyncTemplatePath);
        }
    }
}