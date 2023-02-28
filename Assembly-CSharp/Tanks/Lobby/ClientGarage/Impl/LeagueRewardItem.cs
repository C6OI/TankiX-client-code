using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LeagueRewardItem : MonoBehaviour {
        [SerializeField] ImageSkin itemIcon;

        [SerializeField] TextMeshProUGUI info;

        public string Text {
            set => info.text = value;
        }

        public string Icon {
            set => itemIcon.SpriteUid = value;
        }
    }
}