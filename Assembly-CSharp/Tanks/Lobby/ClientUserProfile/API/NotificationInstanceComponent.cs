using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class NotificationInstanceComponent : Component {
        public NotificationInstanceComponent(GameObject instance) => Instance = instance;

        public GameObject Instance { get; set; }
    }
}