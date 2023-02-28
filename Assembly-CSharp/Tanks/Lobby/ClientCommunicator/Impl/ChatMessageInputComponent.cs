using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatMessageInputComponent : MonoBehaviour, Component {
        void Start() {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}