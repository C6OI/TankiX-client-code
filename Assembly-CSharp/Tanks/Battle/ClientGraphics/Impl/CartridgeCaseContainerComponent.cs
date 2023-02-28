using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [SerialVersionUID(635824352101335226L)]
    public class CartridgeCaseContainerComponent : Component {
        public readonly Queue<GameObject> Cartridges = new();
    }
}