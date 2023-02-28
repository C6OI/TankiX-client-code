using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class GroupNodeCollectorImpl : NodeCollector {
        readonly IDictionary<NodeDescription, ICollection<Entity>> entitiesByDescription;

        public GroupNodeCollectorImpl() => entitiesByDescription = new Dictionary<NodeDescription, ICollection<Entity>>();

        public void Attach(Entity entity, NodeDescription nodeDescription) {
            ICollection<Entity> value;

            if (!entitiesByDescription.TryGetValue(nodeDescription, out value)) {
                value = new HashSet<Entity>();
                entitiesByDescription[nodeDescription] = value;
            }

            value.Add(entity);
        }

        public void Detach(Entity entity, NodeDescription nodeDescription) {
            entitiesByDescription[nodeDescription].Remove(entity);
        }

        public ICollection<Entity> FilterEntities(ICollection<Entity> values, NodeDescription nodeDescription) {
            ICollection<Entity> entities = GetEntities(nodeDescription);
            return values.Where(value => entities.Contains(value)).ToList();
        }

        public ICollection<Entity> GetEntities(NodeDescription nodeDescription) {
            if (nodeDescription.IsEmpty) {
                throw new EmptyNodeNotSupportedException();
            }

            ICollection<Entity> value;

            if (entitiesByDescription.TryGetValue(nodeDescription, out value)) {
                return value;
            }

            return Collections.EmptyList<Entity>();
        }
    }
}