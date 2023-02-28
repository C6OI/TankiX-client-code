using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestWindowTextComponent : LocalizedScreenComponent {
        [SerializeField] TextMeshProUGUI closeButton;

        public string CloseButton {
            set => closeButton.text = value;
        }
    }
}