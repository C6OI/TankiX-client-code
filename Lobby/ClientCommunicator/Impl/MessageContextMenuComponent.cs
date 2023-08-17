using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Lobby.ClientCommunicator.Impl {
    public class MessageContextMenuComponent : MonoBehaviour {
        public Entity SelectedMessage { get; set; }
    }
}