using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class CombatEventLogText : BaseCombatLogMessageElement {
        [SerializeField] TMP_Text text;

        public TMP_Text Text => text;
    }
}