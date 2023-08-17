using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientUserProfile.Impl {
    public class ViewUserEmailScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text yourEmailReplaced;

        [SerializeField] Color emailColor = Color.green;

        public string YourEmailReplaced {
            set => yourEmailReplaced.text = value;
        }

        public string YourEmail { get; set; }

        public Color EmailColor => emailColor;
    }
}