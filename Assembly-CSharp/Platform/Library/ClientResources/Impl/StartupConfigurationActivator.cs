using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Library.ClientResources.Impl {
    public class StartupConfigurationActivator : UnityAwareActivator<AutoCompleting> {
        [Inject] public static YamlService YamlService { get; set; }

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        [Inject] public new static EngineService EngineService { get; set; }

        protected override void Activate() {
            try {
                StartupConfiguration.Config = ConfigurationService.GetConfig(ConfigPath.STARTUP).ConvertTo<StartupConfiguration>();
            } catch (Exception ex) {
                HandleError<InvalidLocalConfigurationErrorEvent>(string.Format("Invalid local configuration. Error: {0}", ex.Message), ex);
            }
        }

        void HandleError<T>(string errorMessage, Exception e) where T : Event, new() {
            LoggerProvider.GetLogger(this).Error(errorMessage, e);
            Engine engine = EngineService.Engine;
            Entity entity = engine.CreateEntity("StartupConfigLoading");
            engine.ScheduleEvent<T>(entity);
        }
    }
}