using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableHeaderComponent : MonoBehaviour, Component {
        public List<ScoreTableRowIndicator> headers = new();
    }
}