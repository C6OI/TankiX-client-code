using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [JoinBy(typeof(ScoreTableGroupComponent))]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    public class JoinByScoreTable : Attribute { }
}