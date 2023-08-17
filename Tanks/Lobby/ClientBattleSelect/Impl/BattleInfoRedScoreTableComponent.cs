using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleInfoRedScoreTableComponent : MonoBehaviour, Component {
        public GameObject rowPrefab;
    }
}