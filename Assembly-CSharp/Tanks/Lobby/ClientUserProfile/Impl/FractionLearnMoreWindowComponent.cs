using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionLearnMoreWindowComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI _competitionTitle;

        [SerializeField] TextMeshProUGUI _competitionDescription;

        [SerializeField] ImageSkin _competitionLogo;

        public string CompetitionTitle {
            set => _competitionTitle.text = value;
        }

        public string CompetitionDescription {
            set => _competitionDescription.text = value;
        }

        public string CompetitionLogoUid {
            set => _competitionLogo.SpriteUid = value;
        }
    }
}