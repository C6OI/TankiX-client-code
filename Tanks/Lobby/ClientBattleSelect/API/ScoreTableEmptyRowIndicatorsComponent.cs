using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableEmptyRowIndicatorsComponent : MonoBehaviour, Component {
        public List<ScoreTableRowIndicator> indicators = new();

        public Color emptyRowColor;

        public Stack<ScoreTableRowComponent> emptyRows = new();
    }
}