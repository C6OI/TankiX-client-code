using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeInstanceCache {
        const int MAX_CACHE_SIZE = 100;

        readonly Stack<Node> cache;

        readonly List<Node> freedInflow;

        readonly Type nodeType;

        public NodeInstanceCache(Type nodeType) {
            cache = new Stack<Node>();
            freedInflow = new List<Node>();
            this.nodeType = nodeType;
        }

        public Node GetInstance() => cache.Count == 0 ? (Node)Activator.CreateInstance(nodeType) : cache.Pop();

        public void Free(Node item) {
            if (freedInflow.Count <= 100) {
                freedInflow.Add(item);
            }
        }

        public void OnFlowClean() {
            for (int i = 0; i < freedInflow.Count; i++) {
                if (cache.Count > 100) {
                    break;
                }

                cache.Push(freedInflow[i]);
            }

            freedInflow.Clear();
        }
    }
}