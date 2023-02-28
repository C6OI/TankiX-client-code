using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class LoadingErrorsSystem : ECSSystem {
        [OnEventFire]
        public void ShowErrorScreen(InvalidLocalConfigurationErrorEvent e, Node node) {
            FatalErrorHandler.ShowBrokenConfigsErrorScreen();
        }

        [OnEventFire]
        public void ShowErrorScreen(NoServerConnectionEvent e, Node node) {
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/noserverconnection");
        }

        [OnEventFire]
        public void ShowErrorScreen(ServerDisconnectedEvent e, Node node) {
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/serverdisconnected");
        }

        [OnEventFire]
        public void ShowErrorScreen(InvalidGameDataErrorEvent e, Node node) {
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/invalid_game_data");
        }

        [OnEventFire]
        public void ShowErrorScreen(GameDataLoadErrorEvent e, Node node) {
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/game_data_load_error");
        }

        [OnEventFire]
        public void ShowErrorScreen(TechnicalWorkEvent e, Node node) {
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/technicalwork");
        }

        [OnEventFire]
        public void ShowCloseReason(ServerConnectionCloseReasonEvent e, Node node) {
            if ("DISABLED".Equals(e.Reason)) {
                FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/technicalwork");
            } else if ("OVERLOADED".Equals(e.Reason)) {
                FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/serveroverloaded");
            }
        }

        [OnEventFire]
        public void ShowErrorScreen(NotEnoughDiskSpaceForCacheErrorEvent e, Node node) {
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/notenoughdiskspaceforcache");
        }

        [OnEventFire]
        public void ShowErrorScreen(DisconectUserFromBlockedCountryEvent e, Node node) {
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/notavailableinregion");
        }
    }
}