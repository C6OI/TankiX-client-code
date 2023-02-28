using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(8116072916726390829L)]
    public interface BonusRegionTemplate : Template {
        BonusRegionComponent BonusRegion();

        SpatialGeometryComponent SpatialGeometry();

        BonusRegionInstanceComponent BonusRegionInstance();

        GoldBonusRegionComponent GoldBonusRegion();

        SupplyBonusRegionComponent SupplyBonusRegion();

        [AutoAdded]
        OpacityBonusRegionComponent OpacityBonusRegion();
    }
}