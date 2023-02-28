using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PopupDialogComponent : ConfirmDialogComponent {
        public RectTransform itemsContainer;

        public LeagueEntranceItemComponent itemPrefab;

        public float itemsShowDelay = 0.6f;

        public ImageSkin leagueIcon;

        public TextMeshProUGUI headerText;

        public TextMeshProUGUI text;

        public TextMeshProUGUI rewardHeader;

        public TextMeshProUGUI buttonText;
    }
}