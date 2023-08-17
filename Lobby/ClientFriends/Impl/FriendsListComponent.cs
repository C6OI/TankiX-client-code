using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientFriends.Impl {
    public class FriendsListComponent : MonoBehaviour, Component {
        [SerializeField] ToggleGroup toggleGroup;

        [SerializeField] GameObject friendsListItem;

        [SerializeField] GameObject acceptedFriendsHeader;

        [SerializeField] GameObject possibleFriendsHeader;

        [SerializeField] GameObject friendsAcceptedList;

        [SerializeField] GameObject friendsIncommingList;

        [SerializeField] GameObject friendsOutgoingList;

        [SerializeField] GameObject emptyListNotify;

        public ToggleGroup ToggleGroup => toggleGroup;

        public GameObject FriendsListItem => friendsListItem;

        public GameObject AcceptedFriendsHeader => acceptedFriendsHeader;

        public GameObject PossibleFriendsHeader => possibleFriendsHeader;

        public GameObject FriendsAcceptedList => friendsAcceptedList;

        public GameObject FriendsIncommingList => friendsIncommingList;

        public GameObject FriendsOutgoingList => friendsOutgoingList;

        public GameObject EmptyListNotify => emptyListNotify;
    }
}