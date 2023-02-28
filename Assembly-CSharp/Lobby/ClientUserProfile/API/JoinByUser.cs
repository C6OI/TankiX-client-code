using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.API {
    [JoinBy(typeof(UserGroupComponent))]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    public class JoinByUser : Attribute { }
}