using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.ClientLauncher.Impl {
    public class LauncherScreensSystem : ECSSystem {
        [OnEventFire]
        [Mandatory]
        public void StartDownload(StartDownloadEvent e, Node any, [JoinAll] DownloadScreenNode screenNode) {
            screenNode.textMapping.Text = screenNode.launcherDownloadScreenText.DownloadText;
        }

        [OnEventFire]
        [Mandatory]
        public void StartReboot(StartRebootEvent e, Node any, [JoinAll] DownloadScreenNode screenNode) {
            screenNode.textMapping.Text = screenNode.launcherDownloadScreenText.RebootText;
        }

        [OnEventFire]
        public void ShowErrorScreen(ClientUpdateErrorEvent e) {
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/clientupdateerror");
        }

        public class DownloadScreenNode : Node {
            public LauncherDownloadScreenComponent launcherDownloadScreen;

            public LauncherDownloadScreenTextComponent launcherDownloadScreenText;

            public TextMappingComponent textMapping;
        }
    }
}