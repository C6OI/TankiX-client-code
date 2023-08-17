using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftAimingStraightDirectionCollectorSystem : AbstractDirectionsCollectorSystem {
        [OnEventFire]
        public void CollectDirection(ShaftAimingCollectDirectionEvent evt, Node weapon) {
            TargetingData targetingData = evt.TargetingData;
            evt.TargetingData.BestDirection = CollectDirection(targetingData.Origin, targetingData.Dir, 0f, targetingData);
        }
    }
}