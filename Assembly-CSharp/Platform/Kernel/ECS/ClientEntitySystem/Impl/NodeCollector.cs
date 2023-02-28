using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface NodeCollector {
        void Attach(Entity entity, NodeDescription nodeDescription);

        void Detach(Entity entity, NodeDescription nodeDescription);

        ICollection<Entity> FilterEntities(ICollection<Entity> values, NodeDescription nodeDescription);

        ICollection<Entity> GetEntities(NodeDescription nodeDescription);
    }
}