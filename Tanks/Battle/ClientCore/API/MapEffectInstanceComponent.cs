using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class MapEffectInstanceComponent : Component {
        public MapEffectInstanceComponent(GameObject instance) => Instance = instance;

        public GameObject Instance { get; set; }
    }
}