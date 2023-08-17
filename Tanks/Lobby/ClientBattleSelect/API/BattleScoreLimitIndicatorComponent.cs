using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class BattleScoreLimitIndicatorComponent : MonoBehaviour, Component {
        [SerializeField] Text scoreLimitText;

        public int ScoreLimit {
            get => int.Parse(scoreLimitText.text);
            set => scoreLimitText.text = value.ToString();
        }
    }
}