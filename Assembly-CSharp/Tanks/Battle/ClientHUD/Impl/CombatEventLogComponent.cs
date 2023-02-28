using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class CombatEventLogComponent : MonoBehaviour, Component {
        public Color NeutralColor;

        public Color AllyColor;

        public Color EnemyColor;

        public Color RedTeamColor;

        public Color BlueTeamColor;
    }
}