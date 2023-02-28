using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientNavigation.API {
    public class TopPanelConstructorComponent : MonoBehaviour, Component {
        [SerializeField] bool showBackground;

        [SerializeField] bool showBackButton;

        [SerializeField] bool showHeader;

        public bool ShowBackground => showBackground;

        public bool ShowBackButton => showBackButton;

        public bool ShowHeader => showHeader;
    }
}