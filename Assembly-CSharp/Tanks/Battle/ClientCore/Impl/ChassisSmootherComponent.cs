using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ChassisSmootherComponent : Component {
        public SimpleValueSmoother maxSpeedSmoother = new(1f, 10f, 0f, 0f);

        public SimpleValueSmoother maxTurnSpeedSmoother = new(54f / (float)Math.PI, 10f, 0f, 0f);
    }
}