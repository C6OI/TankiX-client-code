using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientNavigation.API {
    public class ScreensRegistryComponent : Component {
        public List<GameObject> screens = new();
    }
}