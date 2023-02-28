using System.Collections.Generic;

namespace Edelweiss.DecalSystem {
    internal class CutEdges {
        readonly SortedDictionary<CutEdge, CutEdge> m_CutEdgeDictionary = new();

        public int Count => m_CutEdgeDictionary.Count;

        public CutEdge this[CutEdge a_CutEdge] {
            get => m_CutEdgeDictionary[a_CutEdge];
            set => m_CutEdgeDictionary[a_CutEdge] = value;
        }

        public void Clear() {
            m_CutEdgeDictionary.Clear();
        }

        public bool HasEdge(CutEdge a_CutEdge) => m_CutEdgeDictionary.ContainsKey(a_CutEdge);

        public void AddEdge(CutEdge a_CutEdge) {
            m_CutEdgeDictionary.Add(a_CutEdge, a_CutEdge);
        }
    }
}