using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableRowColorComponent : MonoBehaviour, Component {
        public Color rowColor;

        public Color selfRowColor;
    }
}