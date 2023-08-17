using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientSettings.Impl {
    public class ClientSettingsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [SerializeField] string saturationLevelTemplatePath;

        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new SelectLocaleScreenSystem());
            EngineService.RegisterSystem(new ScreenResolutionSettingsCarouselBuilderSystem());
            EngineService.RegisterSystem(new GraphicsSettingsScreenSystem());
            EngineService.RegisterSystem(new GraphicsSettingsBuilderSystem());
            EngineService.RegisterSystem(new SoundListenerResourcesSystem());
            TemplateRegistry.Register<SettingsTemplate>();
            TemplateRegistry.Register<QualitySettingsVariantTemplate>();
            TemplateRegistry.Register<ScreenResolutionVariantTemplate>();
            TemplateRegistry.Register<WindowModesTemplate>();
            TemplateRegistry.Register<SaturationLevelVariantTemplate>();
            TemplateRegistry.Register<GraphicsSettingsBuilderTemplate>();
            TemplateRegistry.Register<SaturationLevelSettingsBuilderTemplate>();
        }

        protected override void Activate() => ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine e) {
            BuildGraphicsSettings(e);
            e.CreateEntity<SettingsTemplate>(string.Empty);
        });

        void BuildGraphicsSettings(Engine engine) {
            engine.CreateEntity("GraphicsSettingsIndexes").AddComponent<GraphicsSettingsIndexesComponent>();
            engine.CreateEntity<SaturationLevelSettingsBuilderTemplate>(saturationLevelTemplatePath);
        }
    }
}