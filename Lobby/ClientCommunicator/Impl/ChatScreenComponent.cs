using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientCommunicator.Impl {
    [SerialVersionUID(635839578077614070L)]
    public class ChatScreenComponent : MonoBehaviour, Component {
        [SerializeField] GameObject favoritesAndUsers;

        [SerializeField] GameObject messageContextMenu;

        [SerializeField] GameObject activeUserList;

        public GameObject FavoritesAndUsers {
            get => favoritesAndUsers;
            set => favoritesAndUsers = value;
        }

        public GameObject MessageContextMenu {
            get => messageContextMenu;
            set => messageContextMenu = value;
        }

        public GameObject ActiveUserList {
            get => activeUserList;
            set => activeUserList = value;
        }

        void OnEnable() => MessageContextMenu.SetActive(false);
    }
}