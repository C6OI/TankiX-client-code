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

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public class StateServerActivator : UnityAwareActivator<ManuallyCompleting> {
        int state;
        WWWLoader wwwLoader;

        [Inject] public static YamlService yamlService { get; set; }

        void Update() {
            if (wwwLoader == null || !wwwLoader.IsDone) {
                return;
            }

            if (!string.IsNullOrEmpty(wwwLoader.Error)) {
                string errorMessage = string.Format("Configuration loading was failed. URL: {0}, Error: {1}",
                    wwwLoader.URL,
                    wwwLoader.Error);

                HandleError<ServerDisconnectedEvent>(errorMessage);
                return;
            }

            if (wwwLoader.Bytes == null || wwwLoader.Bytes.Length == 0) {
                HandleError<InvalidLoadedDataErrorEvent>("Empty server state data. URL: " + wwwLoader.URL);
                return;
            }

            try {
                using (MemoryStream stream = new(wwwLoader.Bytes)) {
                    StreamReader reader = new(stream);
                    StateConfiguration stateConfiguration = yamlService.Load<StateConfiguration>(reader);
                    state = stateConfiguration.State;

                    if (state != 0) {
                        HandleError<TechnicalWorkEvent>();
                    }
                }
            } catch (Exception ex) {
                HandleError<InvalidLoadedDataErrorEvent>(string.Format("Invalid configuration data. URL: {0}, Error: {1}",
                    wwwLoader.URL,
                    ex.Message));

                return;
            }

            DisposeWWWLoader();
            Complete();
        }

        protected override void Activate() {
            string url = StartupConfiguration.Config.StateUrl + "?rnd=" + new Random().NextDouble();
            wwwLoader = new WWWLoader(new WWW(url));
        }

        void HandleError<T>(string errorMessage) where T : Event, new() {
            LoggerProvider.GetLogger(this).Error(errorMessage);
            HandleError<T>();
        }

        void HandleError<T>() where T : Event, new() {
            DisposeWWWLoader();

            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                Entity entity = engine.CreateEntity("StateServerActivator");
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