using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    public class CombatEventLogUser : BaseCombatLogMessageElement {
        [SerializeField] ImageListSkin rankIcon;

        [SerializeField] Text userName;

        public ImageListSkin RankIcon => rankIcon;

        public Text UserName => userName;
    }
}