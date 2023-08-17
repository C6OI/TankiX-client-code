using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientUserProfile.API {
    public class UserLabelAvatarComponent : MonoBehaviour, Component {
        [SerializeField] GameObject avatarImage;

        [SerializeField] Color offlineColor;

        [SerializeField] Color onlineColor;

        public Color OfflineColor => offlineColor;

        public Color OnlineColor => onlineColor;

        public GameObject AvatarImage => avatarImage;
    }
}