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
        [Inject] public static YamlService yamlService { get; set; }

        [Inject] public static ConfigurationService configurationService { get; set; }

        protected override void Activate() {
            try {
                StartupConfiguration.Config =
                    configurationService.GetConfig(ConfigPath.STARTUP).ConvertTo<StartupConfiguration>();
            } catch (Exception ex) {
                HandleError<InvalidLocalConfigurationErrorEvent>(string.Format("Invalid local configuration. Error: {0}",
                        ex.Message),
                    ex);
            }
        }

        void HandleError<T>(string errorMessage, Exception e) where T : Event, new() {
            LoggerProvider.GetLogger(this).Error(errorMessage, e);
            HandleError<T>();
        }

        void HandleError<T>() where T : Event, new() => ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
            Entity entity = engine.CreateEntity("StartupConfigLoading");
            engine.ScheduleEvent<T>(entity);
        });
    }
}