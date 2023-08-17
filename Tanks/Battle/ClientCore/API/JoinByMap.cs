using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    [JoinBy(typeof(MapGroupComponent))]
    public class JoinByMap : Attribute { }
}