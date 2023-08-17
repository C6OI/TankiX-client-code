using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientSettings.API {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    [JoinBy(typeof(GraphicsSettingsBuilderGroupComponent))]
    public class JoinByGraphicsSettingsBuilderAttribute : Attribute { }
}