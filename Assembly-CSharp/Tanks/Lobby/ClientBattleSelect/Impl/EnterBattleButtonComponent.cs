using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class EnterBattleButtonComponent : MonoBehaviour, Component {
        [SerializeField] TeamColor teamColor;

        public TeamColor TeamColor => teamColor;
    }
}