using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class GoToChangeUserEmailScreenButtonLocalization : LocalizedControl {
        [SerializeField] Text text;

        public override string YamlKey => "changeEmailButton";

        public string Text {
            set => text.text = value.ToUpper();
        }
    }
}