using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class BonusBoxInstanceComponent : Component {
        public GameObject BonusBoxInstance { get; set; }

        public bool Removed { get; set; }
    }
}