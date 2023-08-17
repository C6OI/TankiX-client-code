using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTablePrizeIndicatorComponent : MonoBehaviour, Component {
        [SerializeField] Text prizeText;

        public int Prize {
            get => int.Parse(prizeText.text);
            set => prizeText.text = value.ToString();
        }
    }
}