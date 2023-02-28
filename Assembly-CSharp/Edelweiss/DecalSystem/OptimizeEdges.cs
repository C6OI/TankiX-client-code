using System.Collections.Generic;

namespace Edelweiss.DecalSystem {
    internal class OptimizeEdges {
        readonly SortedDictionary<OptimizeEdge, OptimizeEdge> m_EdgeDictionary = new();

        public int Count => m_EdgeDictionary.Count;

        public OptimizeEdge this[OptimizeEdge a_OptimizeEdge] {
            get => m_EdgeDictionary[a_OptimizeEdge];
            set => m_EdgeDictionary[a_OptimizeEdge] = value;
        }

        public void Clear() {
            m_EdgeDictionary.Clear();
        }

        public bool HasEdge(OptimizeEdge a_OptimizeEdge) => m_EdgeDictionary.ContainsKey(a_OptimizeEdge);

        public void AddEdge(OptimizeEdge a_OptimizeEdge) {
            m_EdgeDictionary.Add(a_OptimizeEdge, a_OptimizeEdge);
        }

        public void RemoveEdge(OptimizeEdge a_OptimizeEdge) {
            m_EdgeDictionary.Remove(a_OptimizeEdge);
        }

        public List<OptimizeEdge> OptimizedEdgeList() {
            List<OptimizeEdge> list = new();

            foreach (OptimizeEdge key in m_EdgeDictionary.Keys) {
                list.Add(key);
            }

            return list;
        }
    }
}