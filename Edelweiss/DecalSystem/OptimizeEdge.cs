using System;

namespace Edelweiss.DecalSystem {
    struct OptimizeEdge : IComparable<OptimizeEdge> {
        public int vertex1Index;

        public int vertex2Index;

        public int triangle1Index;

        public int triangle2Index;

        public OptimizeEdge(int a_Vertex1Index, int a_Vertex2Index, int a_Triangle1Index) {
            if (a_Vertex1Index < a_Vertex2Index) {
                vertex1Index = a_Vertex1Index;
                vertex2Index = a_Vertex2Index;
            } else {
                vertex1Index = a_Vertex2Index;
                vertex2Index = a_Vertex1Index;
            }

            triangle1Index = a_Triangle1Index;
            triangle2Index = -1;
        }

        public int CompareTo(OptimizeEdge a_Other) {
            int num = vertex1Index.CompareTo(a_Other.vertex1Index);

            if (num == 0) {
                num = vertex2Index.CompareTo(a_Other.vertex2Index);
            }

            return num;
        }
    }
}