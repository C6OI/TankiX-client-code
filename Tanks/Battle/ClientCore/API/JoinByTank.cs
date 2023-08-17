using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    [JoinBy(typeof(TankGroupComponent))]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    public class JoinByTank : Attribute { }
}