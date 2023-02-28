using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class UserLabelAvatarComponent : MonoBehaviour, Component {
        [SerializeField] ImageSkin avatarImage;

        [SerializeField] Color offlineColor;

        [SerializeField] Color onlineColor;

        [SerializeField] ImageListSkin _avatarFrame;

        public Color OfflineColor => offlineColor;

        public Color OnlineColor => onlineColor;

        public ImageSkin AvatarImage => avatarImage;

        public bool IsPremium {
            set {
                if ((bool)_avatarFrame) {
                    _avatarFrame.SelectedSpriteIndex = value ? 3 : 0;
                }
            }
        }

        public bool IsSelf {
            set {
                if ((bool)_avatarFrame && value) {
                    _avatarFrame.SelectedSpriteIndex = 1;
                }
            }
        }
    }
}