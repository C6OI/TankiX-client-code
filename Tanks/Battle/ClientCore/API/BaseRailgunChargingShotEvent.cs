using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-7794607793680215703L)]
    public class BaseRailgunChargingShotEvent : TimeValidateEvent { }
}