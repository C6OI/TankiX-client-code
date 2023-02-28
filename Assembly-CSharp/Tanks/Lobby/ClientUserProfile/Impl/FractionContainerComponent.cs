using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionContainerComponent : BehaviourComponent {
        public enum FractionContainerTargets {
            PLAYER_FRACTION = 0,
            WINNER_FRACTION = 1
        }

        [SerializeField] ImageSkin _fractionLogo;

        [SerializeField] TextMeshProUGUI _fractionTitle;

        [SerializeField] CanvasGroup _canvasGroup;

        public FractionContainerTargets Target;

        public string FractionLogoUid {
            set => _fractionLogo.SpriteUid = value;
        }

        public string FractionTitle {
            set => _fractionTitle.text = value;
        }

        public Color FractionColor {
            set => _fractionTitle.color = value;
        }

        public bool IsAvailable {
            set => _canvasGroup.alpha = !value ? 0f : 1f;
        }
    }
}