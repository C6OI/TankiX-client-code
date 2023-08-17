using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public struct NodesToChange {
        public ICollection<NodeDescription> NodesToAdd { get; set; }

        public ICollection<NodeDescription> NodesToRemove { get; set; }

        public void Init() {
            NodesToAdd.Clear();
            NodesToRemove.Clear();
        }

        public NodesToChange Clone(NodesToChange original) {
            NodesToChange result = default;
            result.NodesToAdd = new List<NodeDescription>(original.NodesToAdd);
            result.NodesToRemove = new List<NodeDescription>(original.NodesToRemove);
            return result;
        }
    }
}