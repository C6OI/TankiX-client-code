using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1431673085710L)]
    public class MinePositionComponent : Component {
        public Vector3 Position { get; set; }
    }
}