using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankCommonInstanceComponent : Component {
        public TankCommonInstanceComponent() { }

        public TankCommonInstanceComponent(GameObject tankCommonInstance) => TankCommonInstance = tankCommonInstance;

        public GameObject TankCommonInstance { get; set; }
    }
}