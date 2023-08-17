using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(Text))]
    public class CombatEventLogText : BaseCombatLogMessageElement {
        [SerializeField] Text text;

        public Text Text => text;
    }
}