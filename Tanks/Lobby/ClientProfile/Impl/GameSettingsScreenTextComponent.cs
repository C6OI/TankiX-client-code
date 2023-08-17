using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class GameSettingsScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] Text invertBackwardMovingControl;

        [SerializeField] Text mouseControlAllowed;

        [SerializeField] Text mouseVerticalInverted;

        [SerializeField] Text mouseSensivity;

        public string InvertBackwardMovingControl {
            set => invertBackwardMovingControl.text = value;
        }

        public string MouseControlAllowed {
            set => mouseControlAllowed.text = value;
        }

        public string MouseVerticalInverted {
            set => mouseVerticalInverted.text = value;
        }

        public string MouseSensivity {
            set => mouseSensivity.text = value;
        }
    }
}