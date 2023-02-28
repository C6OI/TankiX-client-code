using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionScoresUiBehaviour : MonoBehaviour {
        [SerializeField] ImageSkin _fractionLogo;

        [SerializeField] TextMeshProUGUI _fractionName;

        [SerializeField] TextMeshProUGUI _fractionScores;

        [SerializeField] GameObject _winnerMark;

        long _scores;

        public string FractionLogoUid {
            set => _fractionLogo.SpriteUid = value;
        }

        public string FractionName {
            set => _fractionName.text = value;
        }

        public Color FractionColor {
            set => _fractionName.color = value;
        }

        public long FractionScores {
            get => _scores;
            set {
                _fractionScores.text = value.ToString();
                _scores = value;
            }
        }

        public bool IsWinner {
            set => _winnerMark.SetActive(value);
        }
    }
}