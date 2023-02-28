using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class CombatEventLogUser : BaseCombatLogMessageElement {
        [SerializeField] ImageListSkin rankIcon;

        [SerializeField] TextMeshProUGUI userName;

        public ImageListSkin RankIcon => rankIcon;

        public TextMeshProUGUI UserName => userName;
    }
}