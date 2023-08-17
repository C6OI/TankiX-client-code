using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false)]
    [JoinBy(typeof(PreviewGroupComponent))]
    public class JoinByPreviewAttribute : Attribute { }
}