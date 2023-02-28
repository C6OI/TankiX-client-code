using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class FlagPointerComponent : CTFPointerComponent {
        public LocalizedField FlagOnTheGroundText;

        public LocalizedField FlagCapturedText;

        public GameObject pointer;

        public void SetFlagOnTheGroundState() {
            text.text = FlagOnTheGroundText.Value;
        }

        public void SetFlagCapturedState() {
            text.text = FlagCapturedText.Value;
        }
    }
}