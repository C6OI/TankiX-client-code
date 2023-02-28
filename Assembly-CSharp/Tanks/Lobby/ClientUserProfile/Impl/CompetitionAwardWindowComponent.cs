using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class CompetitionAwardWindowComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI _fractionNameText;

        [SerializeField] TextMeshProUGUI _fractionRewardDescriptionText;

        [SerializeField] ImageSkin _fractionRewardImage;

        public string FractionName {
            set => _fractionNameText.text = value;
        }

        public Color FractionColor {
            set => _fractionNameText.color = value;
        }

        public string FractionRewardDescription {
            set => _fractionRewardDescriptionText.text = value;
        }

        public string RewardImageUid {
            set => _fractionRewardImage.SpriteUid = value;
        }
    }
}