using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HelpOverlayTextComponent : LocalizedControl {
        [SerializeField] Text controlsText;

        [SerializeField] Text turretText;

        [SerializeField] Text fireText;

        [SerializeField] Text spaceText;

        [SerializeField] Text orText;

        [SerializeField] Text additionalControlsText;

        [SerializeField] Text modulesText;

        [SerializeField] Text graffitiText;

        [SerializeField] Text selfDestructText;

        [SerializeField] Text helpText;

        [SerializeField] Text exitText;

        public string ControlsText {
            set => controlsText.text = value;
        }

        public string TurretText {
            set => turretText.text = value;
        }

        public string FireText {
            set => fireText.text = value;
        }

        public string SpaceText {
            set => spaceText.text = value;
        }

        public string OrText {
            set => orText.text = value;
        }

        public string AdditionalControlsText {
            set => additionalControlsText.text = value;
        }

        public string ModulesText {
            set => modulesText.text = value;
        }

        public string GraffitiText {
            set => graffitiText.text = value;
        }

        public string SelfDestructText {
            set => selfDestructText.text = value;
        }

        public string HelpText {
            set => helpText.text = value;
        }

        public string ExitText {
            set => exitText.text = value;
        }
    }
}