using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeCollectorImpl : NodeCollector {
        readonly Dictionary<NodeDescription, HashSet<Entity>> entitiesByDescription;

        public NodeCollectorImpl() => entitiesByDescription = new Dictionary<NodeDescription, HashSet<Entity>>();

        [Inject] public static FlowInstancesCache Cache { get; set; }

        public void Attach(Entity entity, NodeDescription nodeDescription) {
            HashSet<Entity> value;

            if (!entitiesByDescription.TryGetValue(nodeDescription, out value)) {
                value = new HashSet<Entity>();
                entitiesByDescription.Add(nodeDescription, value);
            }

            value.Add(entity);
        }

        public void Detach(Entity entity, NodeDescription nodeDescription) =>
            entitiesByDescription[nodeDescription].Remove(entity);

        public ICollection<Entity> FilterEntities(ICollection<Entity> values, NodeDescription nodeDescription) {
            if (nodeDescription.IsEmpty) {
                List<Entity> instance = Cache.listEntity.GetInstance();
                Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(values);

                while (enumerator.MoveNext()) {
                    Entity current = enumerator.Current;

                    if (((EntityInternal)current).Alive) {
                        instance.Add(current);
                    }
                }

                return instance;
            }

            if (values.Count == 1) {
                Entity onlyElement = Collections.GetOnlyElement(values);

                if (((EntityInternal)onlyElement).Contains(nodeDescription)) {
                    return Collections.SingletonList(onlyElement);
                }
            }

            HashSet<Entity> value;

            if (entitiesByDescription.TryGetValue(nodeDescription, out value)) {
                List<Entity> instance2 = Cache.listEntity.GetInstance();
                Collections.Enumerator<Entity> enumerator2 = Collections.GetEnumerator(values);

                while (enumerator2.MoveNext()) {
                    Entity current2 = enumerator2.Current;

                    if (value.Contains(current2)) {
                        instance2.Add(current2);
                    }
                }

                return instance2;
            }

            return Collections.EmptyList<Entity>();
        }

        public ICollection<Entity> GetEntities(NodeDescription nodeDescription) {
            if (nodeDescription.IsEmpty) {
                throw new EmptyNodeNotSupportedException();
            }

            HashSet<Entity> value;

            if (entitiesByDescription.TryGetValue(nodeDescription, out value)) {
                return value;
            }

            return Collections.EmptyList<Entity>();
        }
    }
}