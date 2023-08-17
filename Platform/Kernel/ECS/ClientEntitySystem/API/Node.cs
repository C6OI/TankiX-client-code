namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class Node {
        Entity entity;

        public virtual Entity Entity {
            get => entity;
            set => entity = value;
        }

        public override bool Equals(object o) {
            if (this == o) {
                return true;
            }

            if (o is Entity) {
                return entity.Equals(o);
            }

            if (o == null || GetType() != o.GetType()) {
                return false;
            }

            Node node = (Node)o;

            if (entity == null ? node.entity != null : !entity.Equals(node.entity)) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() => entity != null ? entity.GetHashCode() : 0;

        public override string ToString() => string.Format("{0}[Entity={1}]", GetType().Name, Entity);
    }
}