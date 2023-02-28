using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusRegionAssetsComponent : Component {
        public string DoubleDamageAssetGuid { get; set; }

        public string DoubleArmorAssetGuid { get; set; }

        public string RepairKitAssetGuid { get; set; }

        public string SpeedBoostAssetGuid { get; set; }

        public string GoldAssetGuid { get; set; }
    }
}