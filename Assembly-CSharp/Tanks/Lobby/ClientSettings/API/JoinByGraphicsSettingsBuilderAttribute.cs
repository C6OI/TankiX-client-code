using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    [JoinBy(typeof(GraphicsSettingsBuilderGroupComponent))]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    public class JoinByGraphicsSettingsBuilderAttribute : Attribute { }
}