using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class InviteFriendsListComponent : MonoBehaviour, Component {
        [SerializeField] GameObject friendsListItem;

        [SerializeField] GameObject emptyListNotification;

        public GameObject FriendsListItem => friendsListItem;

        public GameObject EmptyListNotification => emptyListNotification;

        public List<string> FriendsUids { get; } = new();
    }
}