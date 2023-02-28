using System;
using System.IO;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientResources.Impl;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.ClientLauncher.Impl {
    public class UpdateConfigurationActivator : UnityAwareActivator<ManuallyCompleting> {
        WWWLoader wwwLoader;

        [Inject] public static YamlService yamlService { get; set; }

        [Inject] public static ConfigurationService configurationService { get; set; }

        void Update() {
            if (wwwLoader == null || !wwwLoader.IsDone) {
                return;
            }

            if (!string.IsNullOrEmpty(wwwLoader.Error)) {
                string errorMessage = string.Format("Update configuration loading was failed. URL: {0}, Error: {1}", wwwLoader.URL, wwwLoader.Error);

                if (wwwLoader.Progress > 0f && wwwLoader.Progress < 1f) {
                    HandleError<ServerDisconnectedEvent>(errorMessage);
                } else {
                    HandleError<NoServerConnectionEvent>(errorMessage);
                }

                return;
            }

            if (wwwLoader.Bytes == null || wwwLoader.Bytes.Length == 0) {
                HandleError<GameDataLoadErrorEvent>("Empty update configuration data. URL: " + wwwLoader.URL);
                return;
            }

            try {
                using (MemoryStream stream = new(wwwLoader.Bytes)) {
                    StreamReader reader = new(stream);
                    UpdateConfiguration config = yamlService.Load<UpdateConfiguration>(reader);
                    UpdateConfiguration.Config = config;
                }
            } catch (Exception ex) {
                HandleError<GameDataLoadErrorEvent>(string.Format("Invalid update configuration data. URL: {0}, Error: {1}", wwwLoader.URL, ex.Message), ex);
                return;
            }

            DisposeWWWLoader();
            Complete();
        }

        protected override void Activate() {
            if (InitConfigurationActivator.LauncherPassed) {
                Complete();
                return;
            }

            string updateConfigUrl = InitConfiguration.Config.UpdateConfigUrl;
            updateConfigUrl = updateConfigUrl.Replace("{DataPath}", Application.dataPath).Replace("{BuildTarget}", BuildTargetName.GetName());
            wwwLoader = new WWWLoader(new WWW(updateConfigUrl));
        }

        void HandleError<T>(string errorMessage) where T : Event, new() {
            LoggerProvider.GetLogger(this).Error(errorMessage);
            HandleError<T>();
        }

        void HandleError<T>(string errorMessage, Exception e) where T : Event, new() {
            LoggerProvider.GetLogger(this).Error(errorMessage, e);
            HandleError<T>();
        }

        void HandleError<T>() where T : Event, new() {
            DisposeWWWLoader();
            Engine engine = EngineService.Engine;
            Entity entity = engine.CreateEntity("UpdateConfigurationLoading");
            engine.ScheduleEvent<T>(entity);
        }

        void DisposeWWWLoader() {
            wwwLoader.Dispose();
            wwwLoader = null;
        }
    }
}