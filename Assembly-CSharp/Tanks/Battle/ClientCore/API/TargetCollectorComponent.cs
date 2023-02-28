using Tanks.Battle.ClientCore.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class TargetCollectorComponent : Component {
        public TargetCollectorComponent(TargetCollector targetCollector, TargetValidator targetValidator) {
            TargetCollector = targetCollector;
            TargetValidator = targetValidator;
        }

        public TargetCollector TargetCollector { get; protected set; }

        public TargetValidator TargetValidator { get; protected set; }

        public DirectionData Collect(Vector3 origin, Vector3 dir, float fullDistance, int layerMask = 0) => TargetCollector.Collect(TargetValidator, fullDistance, origin, dir, layerMask);

        public void Collect(TargetingData targetingData, int layerMask = 0) {
            TargetCollector.Collect(TargetValidator, targetingData, layerMask);
        }

        public void Collect(float fullDistance, DirectionData direction, int layerMask = 0) {
            TargetCollector.Collect(TargetValidator, fullDistance, direction, layerMask);
        }
    }
}