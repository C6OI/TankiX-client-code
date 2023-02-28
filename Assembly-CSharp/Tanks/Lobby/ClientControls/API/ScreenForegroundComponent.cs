using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientControls.API {
    public class ScreenForegroundComponent : MonoBehaviour, Component {
        public int Count { get; set; }
    }
}