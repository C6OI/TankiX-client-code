using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(6803807621463709653L)]
    public class VulcanShootingComponent : TimeValidateComponent {
        public VulcanShootingComponent() { }

        public VulcanShootingComponent(Date startShootingTime) => StartShootingTime = startShootingTime;

        [ProtocolOptional] public Date StartShootingTime { get; set; }
    }
}