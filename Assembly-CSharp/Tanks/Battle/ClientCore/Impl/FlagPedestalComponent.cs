using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(4898317045808451550L)]
    public class FlagPedestalComponent : Component {
        public Vector3 Position { get; set; }
    }
}