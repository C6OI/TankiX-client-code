using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [JoinBy(typeof(SquadGroupComponent))]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    public class JoinBySquad : Attribute { }
}