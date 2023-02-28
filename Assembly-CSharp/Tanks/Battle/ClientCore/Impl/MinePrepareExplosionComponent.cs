using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636377154333752265L)]
    public class MinePrepareExplosionComponent : Component {
        public long PrepareDurationMS { get; set; }

        public Date PrepareExplosionStartTime { get; set; }
    }
}