using Lobby.ClientControls.API;
using UnityEngine;

namespace Lobby.ClientFriends.Impl {
    [RequireComponent(typeof(Animator))]
    public class FriendsListItemComponent : ToggleListItemComponent {
        const string ANIMATOR_SELECTED_KEY = "Selected";

        [SerializeField] GameObject userLabel;

        [SerializeField] GameObject userLabelContainer;

        [SerializeField] GameObject battleLabelContainer;

        [SerializeField] GameObject incomingIcon;

        [SerializeField] GameObject outgoingIcon;

        public FriendsListItemComponent() => onValueChanged += OnValueChanged;

        public GameObject UserLabel => userLabel;

        public GameObject UserLabelContainer => userLabelContainer;

        public GameObject BattleLabelContainer => battleLabelContainer;

        public GameObject IncomingIcon => incomingIcon;

        public GameObject OutgoingIcon => outgoingIcon;

        public void OnValueChanged(bool state) => GetComponent<Animator>().SetBool("Selected", state);
    }
}