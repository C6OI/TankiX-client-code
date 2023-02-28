using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public abstract class ModuleEffectUpgradablePropertyComponent : Component {
        public bool LinearInterpolation { get; set; }

        public float[] UpgradeLevel2Values { get; set; }

        public Dictionary<long, EffectProperty> EquipmentProperties { get; set; }
    }
}