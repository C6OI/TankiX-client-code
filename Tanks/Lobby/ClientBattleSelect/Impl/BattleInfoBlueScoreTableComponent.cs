using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleInfoBlueScoreTableComponent : MonoBehaviour, Component {
        public GameObject rowPrefab;
    }
}