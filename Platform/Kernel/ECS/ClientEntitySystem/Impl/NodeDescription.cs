using System;
using System.Collections.Generic;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface NodeDescription : IComparable<NodeDescription> {
        ICollection<Type> Components { get; }

        ICollection<Type> NotComponents { get; }

        BitSet NodeComponentBitId { get; }

        BitSet NotNodeComponentBitId { get; }

        bool IsEmpty { get; }
    }
}