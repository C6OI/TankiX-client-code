using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    [JoinBy(typeof(MarketItemGroupComponent))]
    public class JoinByMarketItem : Attribute { }
}