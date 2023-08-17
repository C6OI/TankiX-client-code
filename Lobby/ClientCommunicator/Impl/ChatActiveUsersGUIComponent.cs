using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientCommunicator.Impl {
    public class ChatActiveUsersGUIComponent : MonoBehaviour, Component {
        [SerializeField] GameObject userElementPrefab;

        public GameObject UserElementPrefab => userElementPrefab;
    }
}