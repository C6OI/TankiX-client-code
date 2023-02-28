using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EffectTeamGraphicsComponent : MonoBehaviour, Component {
        [SerializeField] Material redTeamMaterial;

        [SerializeField] Material blueTeamMaterial;

        [SerializeField] Material selfMaterial;

        public Material RedTeamMaterial {
            get => redTeamMaterial;
            set => redTeamMaterial = value;
        }

        public Material BlueTeamMaterial {
            get => blueTeamMaterial;
            set => blueTeamMaterial = value;
        }

        public Material SelfMaterial => selfMaterial;
    }
}