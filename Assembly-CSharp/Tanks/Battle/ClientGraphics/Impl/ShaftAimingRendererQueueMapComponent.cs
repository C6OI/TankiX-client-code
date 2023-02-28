using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingRendererQueueMapComponent : Component {
        public ShaftAimingRendererQueueMapComponent() => QueueMap = new Dictionary<Material, int>();

        public Dictionary<Material, int> QueueMap { get; set; }
    }
}