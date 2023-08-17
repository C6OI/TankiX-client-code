using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.API {
    public class UpsideDownTankComponent : Component {
        public Date TimeTankBecomesUpsideDown { get; set; }

        public bool Removed { get; set; }
    }
}