using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableKillsIndicatorComponent : MonoBehaviour, Component {
        [SerializeField] TextMeshProUGUI killsText;

        public int Kills {
            get => int.Parse(killsText.text);
            set => killsText.text = value.ToString();
        }
    }
}