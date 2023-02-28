using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionUserScoreUiComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI _scoreText;

        public long Scores {
            set => _scoreText.text = value.ToString();
        }
    }
}