using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class TankMaterialForDeathComponent : MonoBehaviour, Component {
        public Material мaterial;
    }
}