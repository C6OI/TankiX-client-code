using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AbstractNodeDescription : NodeDescription, IComparable<NodeDescription> {
        public static readonly AbstractNodeDescription EMPTY = new(Collections.EmptyList<Type>());

        readonly int hashCode;

        protected AbstractNodeDescription(ICollection<Type> components)
            : this(components, Collections.EmptyList<Type>()) { }

        protected AbstractNodeDescription(ICollection<Type> components, ICollection<Type> notComponents) {
            Components = components;
            NotComponents = notComponents;
            NodeComponentBitId = new BitSet();
            NotNodeComponentBitId = new BitSet();
            CalcCode(components, NodeComponentBitId);
            CalcCode(notComponents, NotNodeComponentBitId);
            hashCode = CalcGetHashCode();
            IsEmpty = components.Count == 0 && notComponents.Count == 0;
        }

        [Inject] public static ComponentBitIdRegistry ComponentBitIdRegistry { get; set; }

        public bool IsEmpty { get; }

        public BitSet NodeComponentBitId { get; }

        public BitSet NotNodeComponentBitId { get; }

        public ICollection<Type> Components { get; }

        public ICollection<Type> NotComponents { get; }

        public int CompareTo(NodeDescription other) => getKey().CompareTo(((AbstractNodeDescription)other).getKey());

        int CalcGetHashCode() {
            int num = NodeComponentBitId.GetHashCode();
            return 31 * num + NotNodeComponentBitId.GetHashCode();
        }

        void CalcCode(ICollection<Type> components, BitSet componentCode) {
            Collections.Enumerator<Type> enumerator = Collections.GetEnumerator(components);

            while (enumerator.MoveNext()) {
                componentCode.Set(ComponentBitIdRegistry.GetComponentBitId(enumerator.Current));
            }
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (!(obj is AbstractNodeDescription)) {
                return false;
            }

            AbstractNodeDescription abstractNodeDescription = (AbstractNodeDescription)obj;

            if (hashCode != abstractNodeDescription.hashCode) {
                return false;
            }

            if (!NodeComponentBitId.Equals(abstractNodeDescription.NodeComponentBitId)) {
                return false;
            }

            if (!NotNodeComponentBitId.Equals(abstractNodeDescription.NotNodeComponentBitId)) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() => hashCode;

        string getKey() => string.Join(":",
                               (from c in Components
                                select c.FullName
                                into n
                                orderby n
                                select n).ToArray()) +
                           "-NOT-" +
                           string.Join(":",
                               (from c in NotComponents
                                select c.FullName
                                into n
                                orderby n
                                select n).ToArray());

        public override string ToString() => "AbstractNodeDescription components: " +
                                             EcsToStringUtil.ToString(Components) +
                                             " notComponents: " +
                                             EcsToStringUtil.ToString(NotComponents);
    }
}