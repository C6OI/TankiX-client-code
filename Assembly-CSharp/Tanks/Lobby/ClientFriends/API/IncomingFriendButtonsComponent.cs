using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientFriends.API {
    public class IncomingFriendButtonsComponent : BehaviourComponent {
        [SerializeField] GameObject acceptButton;

        [SerializeField] GameObject declineButton;

        EntityBehaviour _acceptEntityBehaviour;

        EntityBehaviour _declineEntityBehaviour;

        EntityBehaviour _mainEntityBehaviour;

        public bool IsIncoming {
            set {
                acceptButton.transform.parent.gameObject.SetActive(value);

                if (value) {
                    Entity entity = _acceptEntityBehaviour.Entity;
                    entity.RemoveComponentIfPresent<UserGroupComponent>();
                    Entity entity2 = _declineEntityBehaviour.Entity;
                    entity2.RemoveComponentIfPresent<UserGroupComponent>();
                    _mainEntityBehaviour.Entity.GetComponent<UserGroupComponent>().Attach(entity).Attach(entity2);
                }
            }
        }

        void OnEnable() {
            _mainEntityBehaviour = GetComponent<EntityBehaviour>();
            _acceptEntityBehaviour = acceptButton.GetComponent<EntityBehaviour>();
            _declineEntityBehaviour = declineButton.GetComponent<EntityBehaviour>();
        }
    }
}