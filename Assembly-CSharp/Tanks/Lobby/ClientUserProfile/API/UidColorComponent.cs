using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class UidColorComponent : MonoBehaviour, Component {
        [SerializeField] Color friendColor;

        [SerializeField] Color moderatorColor;

        [SerializeField] Color color;

        public Color FriendColor {
            get => friendColor;
            set => friendColor = value;
        }

        public Color ModeratorColor {
            get => moderatorColor;
            set => moderatorColor = value;
        }

        public Color Color {
            get => color;
            set => color = value;
        }
    }
}