using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableScoreIndicatorComponent : MonoBehaviour, Component {
        [SerializeField] TextMeshProUGUI scoreText;

        public int Score {
            get => int.Parse(scoreText.text);
            set => scoreText.text = value.ToString();
        }
    }
}