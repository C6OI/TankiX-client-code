using System;
using System.Linq;
using System.Text;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientResources.Impl;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientYaml.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;
using Random = System.Random;

namespace Tanks.Battle.ClientCore.Impl {
    public class StateServerActivator : UnityAwareActivator<ManuallyCompleting> {
        int state;
        WWWLoader wwwLoader;

        [Inject] public static YamlService yamlService { get; set; }

        void Update() {
            if (wwwLoader == null || !wwwLoader.IsDone) {
                return;
            }

            if (!string.IsNullOrEmpty(wwwLoader.Error)) {
                int responseCode = WWWLoader.GetResponseCode(wwwLoader.WWW);

                if (responseCode >= 400) {
                    HandleError<TechnicalWorkEvent>();
                } else {
                    HandleError<NoServerConnectionEvent>(string.Format("Configuration loading was failed. URL: {0}, Error: {1}", wwwLoader.URL, wwwLoader.Error));
                }

                return;
            }

            if (wwwLoader.Bytes == null || wwwLoader.Bytes.Length == 0) {
                HandleError<GameDataLoadErrorEvent>("Empty server state data. URL: " + wwwLoader.URL);
                return;
            }

            string text = string.Empty;

            try {
                text = Encoding.UTF8.GetString(wwwLoader.Bytes);
                StateConfiguration stateConfiguration = yamlService.Load<StateConfiguration>(text);
                state = stateConfiguration.State;

                if (state != 0) {
                    HandleError<TechnicalWorkEvent>();
                }
            } catch (Exception ex) {
                HandleError<GameDataLoadErrorEvent>(string.Format("Invalid configuration data. URL: {0}, Error: {1}, Data: {2}", wwwLoader.URL, ex.Message, text));
                return;
            }

            DisposeWWWLoader();
            Complete();
        }

        protected override void Activate() {
            if (Environment.GetCommandLineArgs().Contains("-ignorestate")) {
                Debug.Log("Ignoring state.yml");
                Complete();
                return;
            }

            if (InitConfigurationActivator.LauncherPassed) {
                Complete();
                return;
            }

            CommandLineParser commandLineParser = new(Environment.GetCommandLineArgs());
            string url = commandLineParser.GetValueOrDefault("-stateUrl", StartupConfiguration.Config.StateUrl) + "?rnd=" + new Random().NextDouble();
            wwwLoader = new WWWLoader(new WWW(url));
            wwwLoader.MaxRestartAttempts = 0;
        }

        void HandleError<T>(string errorMessage) where T : Event, new() {
            LoggerProvider.GetLogger(this).Error(errorMessage);
            HandleError<T>();
        }

        void HandleError<T>() where T : Event, new() {
            DisposeWWWLoader();
            Engine engine = EngineService.Engine;
            Entity entity = engine.CreateEntity("StateServerActivator");
            engine.ScheduleEvent<T>(entity);
        }

        void DisposeWWWLoader() {
            if (wwwLoader != null) {
                wwwLoader.Dispose();
                wwwLoader = null;
            }
        }
    }
}