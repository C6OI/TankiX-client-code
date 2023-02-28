using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientNavigation.API {
    public class LockedElementComponent : MonoBehaviour, Component {
        public CanvasGroup canvasGroup;
    }
}