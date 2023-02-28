using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientLoading.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class OutputLogSystem : ECSSystem {
        const string LOG_PREFIX = "LOG_MARK: ";

        bool firstUserLogged;

        [OnEventFire]
        public void LogSystemInfo(NodeAddedEvent e, SingleNode<LobbyLoadScreenComponent> lobbyLoadScreen) {
            WriteToLog("Device type: " + SystemInfo.deviceType);
            WriteToLog("Graphics device name: " + SystemInfo.graphicsDeviceName);
            WriteToLog("Graphics memory size: " + SystemInfo.graphicsMemorySize + " Mb");
            WriteToLog("Graphics shader model: " + SystemInfo.graphicsShaderLevel);
            WriteToLog("Operating system: " + SystemInfo.operatingSystem);
            WriteToLog("System memory size: ~" + SystemInfo.systemMemorySize + " Mb");
            WriteToLog("Processor type: " + SystemInfo.processorType);
            WriteToLog("Processor count: " + SystemInfo.processorCount);
        }

        [OnEventFire]
        public void LogLobbyLoadComplete(NodeAddedEvent e, LobbyLoadCompletedNode loadCompleted) {
            WriteToLog("Lobby load is completed");
        }

        [OnEventFire]
        public void LogUserReadyToLobby(NodeAddedEvent e, ReadyToLobbyUser user) {
            WriteToLog("User ready to lobby");
        }

        [OnEventFire]
        public void LogUserOnHomeScreen(NodeAddedEvent e, SingleNode<HomeScreenComponent> homeScreen) {
            WriteToLog("User is on Home screen.");
        }

        [OnEventFire]
        public void LogUserTryGoToBattle(NodeAddedEvent e, BattleUserNode user, [JoinByBattle] BattleNode battle, [JoinByMap] MapNode map) {
            WriteToLog("User start going to battle " + map.descriptionItem.Name);
        }

        [OnEventFire]
        public void LogMapInstanceInited(NodeAddedEvent e, SingleNode<MapInstanceComponent> map) {
            WriteToLog("Battle map is inited");
        }

        [OnEventFire]
        public void LogBattleLoadComplete(NodeAddedEvent e, BattleLoadCompletedNode loadCompleted) {
            WriteToLog("Battle load is completed");
        }

        [OnEventFire]
        public void LogUserReadyToBattle(NodeAddedEvent e, ReadyToBattleUser readyToBattleUser) {
            WriteToLog("User is ready to battle");
        }

        [OnEventFire]
        public void LogUserLeftBattle(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser) {
            WriteToLog("User left battle");
        }

        [OnEventFire]
        public void LogLoadScene(LoadSceneEvent e, Node node) {
            WriteToLog(e.SceneName + " scene  start async loading");
        }

        [OnEventFire]
        public void LogClientSessionAdded(NodeAddedEvent e, SingleNode<ClientSessionComponent> clientSession) {
            WriteToLog("Client session added: " + clientSession.Entity.Id);
        }

        [OnEventFire]
        public void LogClientSessionRemoved(NodeRemoveEvent e, SingleNode<ClientSessionComponent> clientSession) {
            WriteToLog("Client session removed: " + clientSession.Entity.Id);
        }

        [OnEventFire]
        public void LogSelfUserAdded(NodeAddedEvent e, SingleNode<SelfUserComponent> user) {
            WriteToLog("Self user added: " + user.Entity.Id);
        }

        [OnEventFire]
        public void LogLobbyAdded(NodeAddedEvent e, SingleNode<LobbyComponent> lobby) {
            WriteToLog("Lobby added");
        }

        [OnEventFire]
        public void LogTopPanelAuthenticatedAdded(NodeAddedEvent e, SingleNode<TopPanelAuthenticatedComponent> topPanel) {
            WriteToLog("TopPanelAuthenticatedComponent added");
        }

        void WriteToLog(string message) {
            Console.WriteLine("LOG_MARK: " + message);
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public SelfBattleUserComponent selfBattleUser;
        }

        public class BattleNode : Node {
            public BattleComponent battle;
            public MapGroupComponent mapGroup;
        }

        public class MapNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MapComponent map;

            public MapGroupComponent mapGroup;
        }

        public class BattleLoadCompletedNode : Node {
            public BattleLoadScreenComponent battleLoadScreen;
            public LoadProgressTaskCompleteComponent loadProgressTaskComplete;
        }

        public class LobbyLoadCompletedNode : Node {
            public LoadProgressTaskCompleteComponent loadProgressTaskComplete;

            public LobbyLoadScreenComponent lobbyLoadScreen;
        }

        public class ReadyToBattleUser : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class ReadyToLobbyUser : Node {
            public SelfUserComponent selfUser;

            public UserReadyForLobbyComponent userReadyForLobby;
        }
    }
}