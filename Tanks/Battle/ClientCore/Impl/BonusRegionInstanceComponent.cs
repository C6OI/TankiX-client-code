using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusRegionInstanceComponent : Component {
        public BonusRegionInstanceComponent() { }

        public BonusRegionInstanceComponent(GameObject bonusRegionInstance) => BonusRegionInstance = bonusRegionInstance;

        public GameObject BonusRegionInstance { get; set; }
    }
}