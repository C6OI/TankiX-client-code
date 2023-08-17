using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientEntrance.API {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    [JoinBy(typeof(UserGroupComponent))]
    public class JoinByUser : Attribute { }
}