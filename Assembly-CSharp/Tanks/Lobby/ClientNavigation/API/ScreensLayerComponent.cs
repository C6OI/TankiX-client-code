using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientNavigation.API {
    public class ScreensLayerComponent : MonoBehaviour, Component {
        public RectTransform selfRectTransform;

        public RectTransform dialogsLayer;

        public RectTransform dialogs60Layer;

        public RectTransform screensLayer;

        public RectTransform screens60Layer;
    }
}