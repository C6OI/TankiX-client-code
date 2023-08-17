using System;

namespace Edelweiss.DecalSystem {
    struct BoneWeightElement : IComparable<BoneWeightElement> {
        public int index;

        public float weight;

        public int CompareTo(BoneWeightElement a_Other) => -weight.CompareTo(a_Other.weight);
    }
}