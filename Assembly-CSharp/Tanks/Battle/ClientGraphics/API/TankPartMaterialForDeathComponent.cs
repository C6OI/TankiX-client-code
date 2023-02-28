using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class TankPartMaterialForDeathComponent : MonoBehaviour, Component {
        [SerializeField] Material[] deathMaterials;

        public Material[] DeathMaterials => deathMaterials;
    }
}