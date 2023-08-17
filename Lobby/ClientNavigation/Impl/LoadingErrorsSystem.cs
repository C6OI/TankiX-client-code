using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientNavigation.Impl {
    public class LoadingErrorsSystem : ECSSystem {
        [OnEventFire]
        public void ShowErrorScreen(InvalidLocalConfigurationErrorEvent e, Node node) =>
            FatalErrorHandler.ShowBrokenResourcesErrorScreen();

        [OnEventFire]
        public void ShowErrorScreen(NoServerConnectionEvent e, Node node) =>
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/noserverconnection");

        [OnEventFire]
        public void ShowErrorScreen(ServerDisconnectedEvent e, Node node) =>
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/serverdisconnected");

        [OnEventFire]
        public void ShowErrorScreen(InvalidLoadedDataErrorEvent e, Node node) =>
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/invalidloadeddata");

        [OnEventFire]
        public void ShowErrorScreen(TechnicalWorkEvent e, Node node) =>
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/technicalwork");

        [OnEventFire]
        public void ShowCloseReason(ServerConnectionCloseReasonEvent e, Node node) {
            if ("DISABLED".Equals(e.Reason)) {
                FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/technicalwork");
            } else if ("OVERLOADED".Equals(e.Reason)) {
                FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/serveroverloaded");
            }
        }

        [OnEventFire]
        public void ShowErrorScreen(NotEnoughDiskSpaceForCacheErrorEvent e, Node node) =>
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/notenoughdiskspaceforcache");
    }
}