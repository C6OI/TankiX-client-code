using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientFriends.API {
    public class FriendsScreenComponent : MonoBehaviour, Component {
        [SerializeField] GameObject searchButton;

        [SerializeField] GameObject profileButton;

        [SerializeField] GameObject battleButton;

        [SerializeField] GameObject acceptButton;

        [SerializeField] GameObject removeButton;

        [SerializeField] GameObject rejectButton;

        [SerializeField] GameObject revokeButton;

        public GameObject SearchButton => searchButton;

        public GameObject ProfileButton => profileButton;

        public GameObject BattleButton => battleButton;

        public GameObject AcceptButton => acceptButton;

        public GameObject RemoveButton => removeButton;

        public GameObject RejectButton => rejectButton;

        public GameObject RevokeButton => revokeButton;
    }
}