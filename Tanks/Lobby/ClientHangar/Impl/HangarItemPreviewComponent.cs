using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarItemPreviewComponent : Component {
        public GameObject Instance { get; set; }
    }
}