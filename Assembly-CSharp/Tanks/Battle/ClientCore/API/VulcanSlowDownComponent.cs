using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-6843896944033144903L)]
    public class VulcanSlowDownComponent : TimeValidateComponent {
        public VulcanSlowDownComponent() { }

        public VulcanSlowDownComponent(bool isAfterShooting) => IsAfterShooting = isAfterShooting;

        public bool IsAfterShooting { get; set; }
    }
}