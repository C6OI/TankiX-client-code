using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientCommunicator.API;
using Tanks.Lobby.ClientCommunicator.Impl;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientLoading.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleLobbyChatSystem : ECSSystem {
        [OnEventFire]
        public void CreateChatUI(NodeAddedEvent e, LobbyChatNode chat) {
            GameObject battleLobbyScreen = MainScreenComponent.Instance.GetComponent<HomeScreenComponent>().BattleLobbyScreen;
            MatchLobbyGUIComponent component = battleLobbyScreen.GetComponent<MatchLobbyGUIComponent>();
            EntityBehaviour component2 = component.chat.GetComponent<EntityBehaviour>();

            if (component2.Entity != null) {
                component2.DestroyEntity();
            }

            EntityBehaviour component3 = component.chat.GetComponent<ChatUIComponent>().MessagesContainer.GetComponent<EntityBehaviour>();

            if (component3.Entity != null) {
                component3.DestroyEntity();
            }

            Entity entity = CreateEntity("LobbyChat");
            component2.BuildEntity(entity);
            Entity entity2 = CreateEntity("LobbyChatContent");
            component3.BuildEntity(entity2);
            chat.Entity.AddComponent<ActiveBattleChannelComponent>();
        }

        [OnEventFire]
        public void DeleteChatUI(NodeRemoveEvent e, LobbyChatNode chat, [JoinAll] ChatUINode chatUI) {
            chat.Entity.RemoveComponent<ActiveBattleChannelComponent>();
            chatUI.battleLobbyChatUI.GetComponent<ChatUIComponent>().MessagesContainer.GetComponent<EntityBehaviour>().DestroyEntity();
            chatUI.battleLobbyChatUI.gameObject.GetComponent<EntityBehaviour>().DestroyEntity();
        }

        [OnEventFire]
        public void GroupChat(NodeAddedEvent e, LobbyUINode lobbyUI, LobbyChatNode chat) {
            lobbyUI.matchLobbyGUI.ShowChat(true);

            if (chat.Entity.HasComponent<ScreenGroupComponent>()) {
                chat.Entity.RemoveComponent<ScreenGroupComponent>();
            }

            lobbyUI.screenGroup.Attach(chat.Entity);
        }

        [OnEventFire]
        public void CleanChatOnBattleStart(NodeAddedEvent e, SingleNode<BattleLoadScreenComponent> battleScreen, [JoinAll] LobbyChatNode chat, [JoinByScreen] ChatUINode chatUI) {
            chatUI.chatUI.MessagesContainer.GetComponent<ChatContentGUIComponent>().ClearMessages();
        }

        public class LobbyChatNode : Node {
            public BattleLobbyChatComponent battleLobbyChat;
            public ChatComponent chat;
        }

        public class LobbyWithBattleGroupNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleLobbyComponent battleLobby;
        }

        public class LobbyUINode : Node {
            public MatchLobbyGUIComponent matchLobbyGUI;

            public ScreenGroupComponent screenGroup;
        }

        public class ChatUINode : Node {
            public BattleLobbyChatUIComponent battleLobbyChatUI;

            public ChatUIComponent chatUI;

            public ScreenGroupComponent screenGroup;
        }

        public class CustomLobbyNode : Node {
            public CustomBattleLobbyComponent customBattleLobby;
        }
    }
}