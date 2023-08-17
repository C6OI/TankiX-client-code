using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableDeathsIndicatorComponent : MonoBehaviour, Component {
        [SerializeField] Text deathsText;

        public int Deaths {
            get => int.Parse(deathsText.text);
            set => deathsText.text = value.ToString();
        }
    }
}