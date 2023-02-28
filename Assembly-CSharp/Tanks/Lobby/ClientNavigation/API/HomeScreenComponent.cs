using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientNavigation.API {
    public class HomeScreenComponent : MonoBehaviour, Component {
        [SerializeField] Text uidText;

        [SerializeField] GameObject cbqBadge;

        [SerializeField] GameObject battleLobbyScreen;

        public virtual string UidText {
            get => uidText.text;
            set => uidText.text = value;
        }

        public virtual GameObject CbqBadge => cbqBadge;

        public virtual GameObject BattleLobbyScreen => battleLobbyScreen;
    }
}