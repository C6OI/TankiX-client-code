using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(4963057750170414217L)]
    public class SelfRailgunChargingShotEvent : BaseRailgunChargingShotEvent { }
}