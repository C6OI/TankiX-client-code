using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class TankPartIntersectionWithCameraMapComponent : Component {
        public TankPartIntersectionWithCameraMapComponent(TankPartIntersectionWithCameraData[] tankPartIntersectionMap) => TankPartIntersectionMap = tankPartIntersectionMap;

        public TankPartIntersectionWithCameraData[] TankPartIntersectionMap { get; set; }
    }
}