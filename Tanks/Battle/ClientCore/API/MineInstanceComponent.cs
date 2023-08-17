using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class MineInstanceComponent : Component {
        public MineInstanceComponent() { }

        public MineInstanceComponent(GameObject gameObject) => GameObject = gameObject;

        public GameObject GameObject { get; set; }
    }
}