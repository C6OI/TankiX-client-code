using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(-7424433796811681217L)]
    [Shared]
    public class FlagPositionComponent : Component {
        public Vector3 Position { get; set; }
    }
}