using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingForceFieldReadyComponent : Component {
        public ShaftAimingForceFieldReadyComponent(int propertyId) => PropertyID = propertyId;

        public int PropertyID { get; set; }
    }
}