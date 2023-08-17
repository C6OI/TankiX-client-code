using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientSettings.API {
    public class SelectedLocaleComponent : MonoBehaviour, Component {
        public string Code { get; set; }
    }
}