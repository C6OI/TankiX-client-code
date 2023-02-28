using Lobby.ClientUserProfile.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientFriends.API {
    public class SendInviteToLobbyButtonComponent : BehaviourComponent {
        public void AttachToUserGroup() {
            GetComponentInParent<UserLabelComponent>().GetComponent<EntityBehaviour>().Entity.GetComponent<UserGroupComponent>().Attach(GetComponent<EntityBehaviour>().Entity);
        }
    }
}