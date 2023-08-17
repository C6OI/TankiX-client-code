using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableKillsIndicatorComponent : MonoBehaviour, Component {
        [SerializeField] Text killsText;

        public int Kills {
            get => int.Parse(killsText.text);
            set => killsText.text = value.ToString();
        }
    }
}