using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class BonusParachuteInstanceComponent : Component {
        public BonusParachuteInstanceComponent(GameObject bonusParachuteInstance) => BonusParachuteInstance = bonusParachuteInstance;

        public GameObject BonusParachuteInstance { get; set; }
    }
}