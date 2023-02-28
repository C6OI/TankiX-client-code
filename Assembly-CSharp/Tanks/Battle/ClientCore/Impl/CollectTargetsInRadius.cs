using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class CollectTargetsInRadius : Event {
        public float Radius;

        public CollectTargetsInRadius() => Targets = new List<Entity>();

        public List<Entity> Targets { get; set; }
    }
}