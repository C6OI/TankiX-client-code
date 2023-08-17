using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientCommunicator.API {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    [JoinBy(typeof(ChatGroupComponent))]
    public class JoinByChat : Attribute { }
}