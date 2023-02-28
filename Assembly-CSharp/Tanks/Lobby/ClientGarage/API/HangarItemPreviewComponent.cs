using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.API {
    public class HangarItemPreviewComponent : Component {
        public GameObject Instance { get; set; }
    }
}