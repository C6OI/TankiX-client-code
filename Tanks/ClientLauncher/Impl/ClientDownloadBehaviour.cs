using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.ClientLauncher.API;
using UnityEngine;

namespace Tanks.ClientLauncher.Impl {
    public class ClientDownloadBehaviour : MonoBehaviour {
        string executable;

        ProgressBar progressBar;

        string updatePath;

        string url;
        string version;

        WWWLoader www;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        void Start() => StartCoroutine(WaitAndStartDownLoad(0.5f));

        void Update() {
            if (www != null) {
                progressBar.ProgressValue = www.Progress;

                if (www.IsDone) {
                    progressBar.ProgressValue = 1f;
                    CompleteDownloading();
                    www.Dispose();
                    www = null;
                }
            }
        }

        public void Init(string version, string url, string executable) {
            updatePath = Path.GetTempPath() + "/" + LauncherConstants.UPDATE_PATH;
            this.url = url;
            this.version = version;
            this.executable = executable;
        }

        IEnumerator WaitAndStartDownLoad(float waitTime) {
            yield return new WaitForSeconds(waitTime);

            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                engine.NewEvent<StartDownloadEvent>().Attach(EngineService.EntityStub).Schedule();
            });

            progressBar = GetComponentInChildren<ProgressBar>();
            progressBar.ProgressValue = 0f;
            www = new WWWLoader(new WWW(url));
        }

        void CompleteDownloading() {
            if (string.IsNullOrEmpty(www.Error)) {
                try {
                    ExtractFiles();
                    Reboot();
                    return;
                } catch (Exception exception) {
                    LoggerProvider.GetLogger(this).Error("Client update was failed. ", exception);
                    SendErrorEvent();
                    return;
                }
            }

            enabled = false;

            LoggerProvider.GetLogger(this).Error(string.Format("Updated client loading was failed. URL: {0}, Error: {1}",
                www.URL,
                www.Error));

            SendErrorEvent();
        }

        void ExtractFiles() {
            try {
                if (Directory.Exists(updatePath)) {
                    FileUtils.DeleteDirectory(updatePath);
                }
            } catch (Exception) {
                updatePath += "_alt";
            }

            try {
                if (Directory.Exists(updatePath)) {
                    FileUtils.DeleteDirectory(updatePath);
                }
            } catch (Exception ex2) {
                LoggerProvider.GetLogger(this).Error(ex2.Message, ex2);
            }

            using (MemoryStream stream = new(www.Bytes)) {
                TarUtility.Extract(stream, updatePath);
            }
        }

        void Reboot() {
            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                engine.NewEvent<StartRebootEvent>().Attach(EngineService.EntityStub).Schedule();
            });

            string appRootPath = ApplicationUtils.GetAppRootPath();

            string args = string.Format("-batchmode -nographics {0}={1} {2}={3} {4}={5}",
                LauncherConstants.UPDATE_PROCESS_COMMAND,
                Process.GetCurrentProcess().Id,
                LauncherConstants.VERSION_COMMAND,
                version,
                LauncherConstants.PARENT_PATH_COMMAND,
                ApplicationUtils.WrapPath(appRootPath));

            ApplicationUtils.StartProcessAsAdmin(updatePath + "/" + ApplicationUtils.GetExecutablePathByName(executable),
                args);

            StartCoroutine(WaitAndReboot(2f));
        }

        IEnumerator WaitAndReboot(float waitTime) {
            yield return new WaitForSeconds(waitTime);

            Application.Quit();
        }

        void SendErrorEvent() => ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
            engine.NewEvent<ClientUpdateErrorEvent>().Schedule();
        });
    }
}