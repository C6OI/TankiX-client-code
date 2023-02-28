using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatMessageClickEvent : Event {
        public PointerEventData EventData { get; set; }

        public string Link { get; set; }
    }
}