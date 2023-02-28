using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class InviteFriendListItemComponent : MonoBehaviour, Component {
        [SerializeField] GameObject userLabelContainer;

        [SerializeField] GameObject battleLabelContainer;

        [SerializeField] GameObject notificationContainer;

        [SerializeField] Text notificationText;

        public GameObject UserLabelContainer => userLabelContainer;

        public GameObject BattleLabelContainer => battleLabelContainer;

        public GameObject NotificationContainer => notificationContainer;

        public Text NotificationText => notificationText;
    }
}