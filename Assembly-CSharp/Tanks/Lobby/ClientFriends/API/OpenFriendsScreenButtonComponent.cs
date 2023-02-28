using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientFriends.API {
    public class OpenFriendsScreenButtonComponent : MonoBehaviour, Component {
        public CountingBadgeComponent countingBadge;
    }
}