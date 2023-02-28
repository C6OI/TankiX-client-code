using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class DMScoreHUDComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI place;

        [SerializeField] TextMeshProUGUI playerScore;

        int _maxScore;

        int _place;

        int _players;

        int _playerScore;

        public int Place {
            get => _place;
            set {
                _place = value;
                UpdatePlayerPlace();
            }
        }

        public int Players {
            get => _players;
            set {
                _players = value;
                UpdatePlayerPlace();
            }
        }

        public int PlayerScore {
            get => _playerScore;
            set {
                _playerScore = value;
                UpdatePlayerScore();
            }
        }

        public int MaxScore {
            get => _maxScore;
            set {
                _maxScore = value;
                UpdatePlayerScore();
            }
        }

        void OnDisable() {
            gameObject.SetActive(false);
        }

        public void UpdatePlayerPlace() {
            place.text = string.Format("{0}<size=12>/{1}</size>", _place, _players);
        }

        public void UpdatePlayerScore() {
            playerScore.text = string.Format("{0}<size=12>/{1}</size>", _playerScore, _maxScore);
        }
    }
}