using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class KeyboardSettingsScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] TextMeshProUGUI mouseSensivity;

        [SerializeField] TextMeshProUGUI invertBackwardMovingControl;

        [SerializeField] TextMeshProUGUI mouseControlAllowed;

        [SerializeField] TextMeshProUGUI mouseVerticalInverted;

        [SerializeField] TextMeshProUGUI keyboardHeader;

        [SerializeField] TextMeshProUGUI tip;

        [SerializeField] TextMeshProUGUI fewActionsError;

        [SerializeField] TextMeshProUGUI modules;

        [SerializeField] Text turretLeft;

        [SerializeField] Text turretRight;

        [SerializeField] Text centerTurret;

        [SerializeField] Text graffiti;

        [SerializeField] Text help;

        [SerializeField] Text changeHud;

        [SerializeField] Text dropFlag;

        [SerializeField] Text cameraUp;

        [SerializeField] Text cameraDown;

        [SerializeField] Text scoreBoard;

        [SerializeField] Text selfDestruction;

        [SerializeField] Text pause;

        public string MouseSensivity {
            set => mouseSensivity.text = value;
        }

        public string InvertBackwardMovingControl {
            set => invertBackwardMovingControl.text = value;
        }

        public string MouseControlAllowed {
            set => mouseControlAllowed.text = value;
        }

        public string MouseVerticalInverted {
            set => mouseVerticalInverted.text = value;
        }

        public string Keyboard {
            set => keyboardHeader.text = value;
        }

        public string Tip {
            set => tip.text = value;
        }

        public string FewActionsError {
            set => fewActionsError.text = value;
        }

        public string Modules {
            set => modules.text = value;
        }

        public virtual string TurretLeft {
            set => turretLeft.text = value;
        }

        public virtual string TurretRight {
            set => turretRight.text = value;
        }

        public virtual string CenterTurret {
            set => centerTurret.text = value;
        }

        public virtual string Graffiti {
            set => graffiti.text = value;
        }

        public virtual string Help {
            set => help.text = value;
        }

        public virtual string ChangeHud {
            set => changeHud.text = value;
        }

        public virtual string DropFlag {
            set => dropFlag.text = value;
        }

        public virtual string CameraUp {
            set => cameraUp.text = value;
        }

        public virtual string CameraDown {
            set => cameraDown.text = value;
        }

        public virtual string ScoreBoard {
            set => scoreBoard.text = value;
        }

        public virtual string SelfDestruction {
            set => selfDestruction.text = value;
        }

        public virtual string Pause {
            set => pause.text = value;
        }
    }
}