using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableEmptyRowIndicatorComponent : MonoBehaviour, Component {
        [SerializeField] Text text;

        public string Text {
            get => text.text;
            set => text.text = value;
        }
    }
}