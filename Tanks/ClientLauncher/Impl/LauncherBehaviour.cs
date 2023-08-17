using System;
using System.IO;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Tanks.ClientLauncher.API;
using UnityEngine;

namespace Tanks.ClientLauncher.Impl {
    public class LauncherBehaviour : MonoBehaviour {
        static LauncherBehaviour instance;

        [SerializeField] GameObject downloadScreen;

        [SerializeField] GameObject errorDialogScreen;

        string currentVersion;

        string distributionUrl;

        string executable;

        string remoteVersion;

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        void Awake() => instance = this;

        public void Launch() {
            ReadConfigs();

            if (CheckUpdateReport()) {
                UpdateClientOrStartGame();
            }
        }

        public static void RetryUpdate() => instance.UpdateClientOrStartGame();

        void UpdateClientOrStartGame() {
            if (remoteVersion != currentVersion) {
                StartClientDownload();
            } else {
                StartGame();
            }
        }

        void ReadConfigs() {
            currentVersion = ConfigurationService.GetConfig(ConfigPath.STARTUP).GetStringValue("currentClientVersion");
            remoteVersion = UpdateConfiguration.Config.LastClientVersion;
            distributionUrl = UpdateConfiguration.Config.DistributionUrl;
            executable = UpdateConfiguration.Config.Executable;
        }

        bool CheckUpdateReport() {
            CommandLineParser commandLineParser = new(Environment.GetCommandLineArgs());

            if (!commandLineParser.IsExist(LauncherConstants.UPDATE_REPORT_COMMAND)) {
                return true;
            }

            UpdateReport updateReport = new();
            string arg = string.Empty;
            string text = string.Empty;
            bool flag;

            try {
                using (FileStream stream = new(ApplicationUtils.GetAppRootPath() + "/" + LauncherConstants.REPORT_FILE_NAME,
                           FileMode.Open)) {
                    updateReport.Read(stream);
                    flag = updateReport.IsSuccess;
                    arg = updateReport.Error + updateReport.StackTrace;
                    text = updateReport.UpdateVersion;
                }
            } catch (Exception ex) {
                flag = false;
                arg = ex.ToString();
            }

            if (!flag) {
                LoggerProvider.GetLogger(this).ErrorFormat("Update Error:{0}", arg);

                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                    engine.NewEvent<ClientUpdateErrorEvent>().Schedule();
                });

                return false;
            }

            if (text != currentVersion) {
                LoggerProvider.GetLogger(this)
                    .ErrorFormat("Update Error: Updated version is not correct, update version = {0}, currentVersion = {1}",
                        updateReport.UpdateVersion,
                        currentVersion);

                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                    engine.NewEvent<ClientUpdateErrorEvent>().Schedule();
                });

                return false;
            }

            return true;
        }

        void StartClientDownload() {
            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                engine.ScheduleEvent<ShowScreenNoAnimationEvent<LauncherDownloadScreenComponent>>(
                    engine.CreateEntity("Launcher"));
            });

            ClientDownloadBehaviour component = downloadScreen.GetComponent<ClientDownloadBehaviour>();
            component.Init(remoteVersion, distributionUrl, executable);
        }

        void StartGame() => ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
            Entity entity = engine.CreateEntity("StartGame");
            engine.ScheduleEvent<SwitchToEntranceSceneEvent>(entity);
        });
    }
}