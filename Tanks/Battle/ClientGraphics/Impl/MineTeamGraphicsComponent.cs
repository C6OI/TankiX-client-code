using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MineTeamGraphicsComponent : MonoBehaviour, Component {
        [SerializeField] Material redTeamMaterial;

        [SerializeField] Material blueTeamMaterial;

        [SerializeField] Material enemyMaterial;

        public Material EnemyMaterial {
            get => enemyMaterial;
            set => enemyMaterial = value;
        }

        public Material RedTeamMaterial {
            get => redTeamMaterial;
            set => redTeamMaterial = value;
        }

        public Material BlueTeamMaterial {
            get => blueTeamMaterial;
            set => blueTeamMaterial = value;
        }
    }
}