using System;
using System.IO;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientYaml.API;
using Tanks.ClientLauncher.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;
using Random = System.Random;

namespace Platform.Library.ClientResources.Impl {
    public class InitConfigurationActivator : UnityAwareActivator<ManuallyCompleting> {
        WWWLoader wwwLoader;

        [Inject] public static YamlService yamlService { get; set; }

        [Inject] public new static EngineService EngineService { get; set; }

        public static bool LauncherPassed { get; set; }

        void Update() {
            if (wwwLoader == null || !wwwLoader.IsDone) {
                return;
            }

            if (!string.IsNullOrEmpty(wwwLoader.Error)) {
                int responseCode = WWWLoader.GetResponseCode(wwwLoader.WWW);

                if (responseCode >= 400) {
                    HandleError<TechnicalWorkEvent>();
                } else {
                    HandleError<NoServerConnectionEvent>(string.Format("Initial config loading was failed. URL: {0}, Error: {1}", wwwLoader.URL, wwwLoader.Error));
                }

                return;
            }

            if (wwwLoader.Bytes == null || wwwLoader.Bytes.Length == 0) {
                HandleError<GameDataLoadErrorEvent>("Initial config is empty. URL: " + wwwLoader.URL);
                return;
            }

            try {
                using (MemoryStream stream = new(wwwLoader.Bytes)) {
                    StreamReader reader = new(stream);
                    InitConfiguration config = yamlService.Load<InitConfiguration>(reader);
                    InitConfiguration.Config = config;
                }
            } catch (Exception ex) {
                HandleError<GameDataLoadErrorEvent>(string.Format("Invalid initial config. URL: {0}, Error: {1}", wwwLoader.URL, ex.Message), ex);
                return;
            }

            DisposeWWWLoader();
            Complete();
        }

        protected override void Activate() {
            if (LauncherPassed) {
                Complete();
                return;
            }

            wwwLoader = new WWWLoader(new WWW(getInitUrl()));
            wwwLoader.MaxRestartAttempts = 0;
        }

        string getInitUrl() {
            CommandLineParser commandLineParser = new(Environment.GetCommandLineArgs());
            string paramValue;

            string text = !commandLineParser.TryGetValue(LauncherConstants.TEST_SERVER, out paramValue) ? StartupConfiguration.Config.InitUrl
                              : "http://" + paramValue + ".test.tankix.com/config/init.yml";

            return text + "?rnd=" + new Random().NextDouble();
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
            Entity entity = engine.CreateEntity("InitConfigLoading");
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