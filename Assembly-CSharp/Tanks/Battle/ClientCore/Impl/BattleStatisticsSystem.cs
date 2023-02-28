using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Battle.ClientCore.Impl {
    public class BattleStatisticsSystem : ECSSystem {
        static bool firstLoad = true;

        [OnEventFire]
        public void LogFirstGarageEntrance(NodeAddedEvent e, SingleNode<MainScreenComponent> homeScreen, [JoinAll] SessionNode session) {
            if (firstLoad) {
                firstLoad = false;
                ScheduleEvent<ClientGarageFirstLoadEvent>(session);
            }
        }

        public class SessionNode : Node {
            public ClientSessionComponent clientSession;
        }
    }
}