using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class UITeamComponent : MonoBehaviour, Component {
        [SerializeField] TeamColor teamColor;

        public TeamColor TeamColor {
            get => teamColor;
            set => teamColor = value;
        }
    }
}