using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AcceleratedGearsInstanceComponent : Component {
        public AcceleratedGearsInstanceComponent(GameObject instance) => Instance = instance;

        public GameObject Instance { get; set; }
    }
}