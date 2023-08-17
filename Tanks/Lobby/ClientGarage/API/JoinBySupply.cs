using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    [JoinBy(typeof(SupplyGroupComponent))]
    public class JoinBySupply : Attribute { }
}