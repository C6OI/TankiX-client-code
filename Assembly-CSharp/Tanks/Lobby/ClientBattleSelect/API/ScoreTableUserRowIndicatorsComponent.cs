using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableUserRowIndicatorsComponent : MonoBehaviour, Component {
        public List<ScoreTableRowIndicator> indicators = new();
    }
}