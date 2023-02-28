using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionDescriptionBehaviour : MonoBehaviour {
        [SerializeField] TextMeshProUGUI _fractionTitle;

        [SerializeField] TextMeshProUGUI _fractionSlogan;

        [SerializeField] TextMeshProUGUI _fractionDescription;

        [SerializeField] ImageSkin _fractionLogo;

        [SerializeField] FractionButtonComponent[] _fractionButtons;

        public string FractionTitle {
            set => _fractionTitle.text = value;
        }

        public string FractionSlogan {
            set => _fractionSlogan.text = value;
        }

        public string FractionDescription {
            set => _fractionDescription.text = value;
        }

        public string LogoUid {
            set => _fractionLogo.SpriteUid = value;
        }

        public Entity FractionId {
            set {
                FractionButtonComponent[] fractionButtons = _fractionButtons;

                foreach (FractionButtonComponent fractionButtonComponent in fractionButtons) {
                    fractionButtonComponent.FractionEntity = value;
                }
            }
        }
    }
}