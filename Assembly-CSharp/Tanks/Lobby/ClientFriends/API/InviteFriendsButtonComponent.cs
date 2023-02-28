using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.API {
    public class InviteFriendsButtonComponent : BehaviourComponent {
        [SerializeField] RectTransform popupPosition;

        public Vector3 PopupPosition => popupPosition.position;
    }
}