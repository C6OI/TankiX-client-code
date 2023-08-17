using Lobby.ClientNavigation.API;
using Lobby.ClientNavigation.API.Screens;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.ClientLauncher.Impl {
    public class LauncherScreensSystem : ECSSystem {
        [OnEventFire]
        [Mandatory]
        public void StartDownload(StartDownloadEvent e, Node any, [JoinAll] DownloadScreenNode screenNode) =>
            screenNode.textMapping.Text = screenNode.launcherDownloadScreenText.DownloadText;

        [Mandatory]
        [OnEventFire]
        public void StartReboot(StartRebootEvent e, Node any, [JoinAll] DownloadScreenNode screenNode) =>
            screenNode.textMapping.Text = screenNode.launcherDownloadScreenText.RebootText;

        [OnEventFire]
        public void ShowErrorScreen(ClientUpdateErrorEvent e) {
            Entity entity = CreateEntity(typeof(ErrorScreenTemplate), "clientlocal/ui/screen/error/clientupdateerror");
            entity.AddComponent(new ErrorScreenActionComponent(Application.Quit));
            ShowScreenNoAnimationEvent<ErrorScreenComponent> showScreenNoAnimationEvent = new();
            showScreenNoAnimationEvent.SetContext(entity, true);
            ScheduleEvent(showScreenNoAnimationEvent, entity);
        }

        public class DownloadScreenNode : Node {
            public LauncherDownloadScreenComponent launcherDownloadScreen;

            public LauncherDownloadScreenTextComponent launcherDownloadScreenText;

            public TextMappingComponent textMapping;
        }
    }
}