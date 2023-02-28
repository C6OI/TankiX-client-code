using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankOpaqueShaderBlockersComponent : Component {
        public TankOpaqueShaderBlockersComponent() => Blockers = new HashSet<string>();

        public HashSet<string> Blockers { get; set; }
    }
}