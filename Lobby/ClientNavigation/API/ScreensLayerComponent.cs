using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientNavigation.API {
    public class ScreensLayerComponent : MonoBehaviour, Component {
        public RectTransform dialogsLayer;

        public RectTransform screensLayer;
    }
}