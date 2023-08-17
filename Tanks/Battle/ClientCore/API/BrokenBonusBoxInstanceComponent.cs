using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class BrokenBonusBoxInstanceComponent : Component {
        public BrokenBonusBoxInstanceComponent() { }

        public BrokenBonusBoxInstanceComponent(GameObject instance) => Instance = instance;

        public GameObject Instance { get; set; }
    }
}