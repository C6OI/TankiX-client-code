using System;
using System.IO;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientYaml.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;
using Random = System.Random;

namespace Platform.Library.ClientResources.Impl {
    public class InitConfigurationActivator : UnityAwareActivator<ManuallyCompleting> {
        WWWLoader wwwLoader;

        [Inject] public static YamlService yamlService { get; set; }

        void Update() {
            if (wwwLoader == null || !wwwLoader.IsDone) {
                return;
            }

            if (!string.IsNullOrEmpty(wwwLoader.Error)) {
                string errorMessage = string.Format("Initial config loading was failed. URL: {0}, Error: {1}",
                    wwwLoader.URL,
                    wwwLoader.Error);

                if (wwwLoader.Progress > 0f && wwwLoader.Progress < 1f) {
                    HandleError<ServerDisconnectedEvent>(errorMessage);
                } else {
                    HandleError<NoServerConnectionEvent>(errorMessage);
                }

                return;
            }

            if (wwwLoader.Bytes == null || wwwLoader.Bytes.Length == 0) {
                HandleError<InvalidLoadedDataErrorEvent>("Initial config is empty. URL: " + wwwLoader.URL);
                return;
            }

            try {
                using (MemoryStream stream = new(wwwLoader.Bytes)) {
                    StreamReader reader = new(stream);
                    InitConfiguration config = yamlService.Load<InitConfiguration>(reader);
                    InitConfiguration.Config = config;
                }
            } catch (Exception ex) {
                HandleError<InvalidLoadedDataErrorEvent>(string.Format("Invalid initial config. URL: {0}, Error: {1}",
                        wwwLoader.URL,
                        ex.Message),
                    ex);

                return;
            }

            DisposeWWWLoader();
            Complete();
        }

        protected override void Activate() {
            string url = StartupConfiguration.Config.InitUrl + "?rnd=" + new Random().NextDouble();
            wwwLoader = new WWWLoader(new WWW(url));
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

            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                Entity entity = engine.CreateEntity("InitConfigLoading");
                engine.ScheduleEvent<T>(entity);
            });
        }

        void DisposeWWWLoader() {
            if (wwwLoader != null) {
                wwwLoader.Dispose();
                wwwLoader = null;
            }
        }
    }
}