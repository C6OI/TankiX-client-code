using Lobby.ClientUserProfile.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.API {
    public class OutgoingFriendButtonsComponent : BehaviourComponent {
        [SerializeField] GameObject revokeButton;

        public bool IsOutgoing {
            set {
                revokeButton.transform.parent.gameObject.SetActive(value);

                if (value) {
                    GetComponent<EntityBehaviour>().Entity.GetComponent<UserGroupComponent>().Attach(revokeButton.GetComponent<EntityBehaviour>().Entity);
                }
            }
        }
    }
}