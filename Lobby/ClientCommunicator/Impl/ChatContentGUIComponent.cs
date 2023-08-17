using UnityEngine;
using UnityEngine.Serialization;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientCommunicator.Impl {
    public class ChatContentGUIComponent : MonoBehaviour, Component {
        [FormerlySerializedAs("messageAsset")] [SerializeField]
        GameObject messagePrefab;

        public GameObject MessagePrefab => messagePrefab;
    }
}