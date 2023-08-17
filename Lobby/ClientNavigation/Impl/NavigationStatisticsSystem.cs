using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientNavigation.Impl {
    public class NavigationStatisticsSystem : ECSSystem {
        [OnEventFire]
        public void SendEnterScreen(NodeAddedEvent e, ActiveScreenNode screen, [JoinAll] ClientSessionNode clientSession) =>
            ScheduleEvent(new EnterScreenEvent(screen.screen.gameObject.name), clientSession);

        public class ActiveScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        public class ClientSessionNode : Node {
            public ClientSessionComponent clientSession;
        }
    }
}