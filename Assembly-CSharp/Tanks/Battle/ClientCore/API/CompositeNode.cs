using System.Collections.Generic;

namespace Tanks.Battle.ClientCore.API {
    public abstract class CompositeNode : BehaviourTreeNode {
        public List<BehaviourTreeNode> children;

        public int currentChildIndex;

        protected CompositeNode() => children = new List<BehaviourTreeNode>();

        public override void Start() {
            currentChildIndex = 0;
        }

        public override void End() {
            currentChildIndex = 0;
        }

        public new void Reset() {
            base.Reset();
            currentChildIndex = 0;

            for (int i = 0; i < children.Count; i++) {
                children[i].Reset();
            }
        }

        public void AddChild(BehaviourTreeNode child) {
            children.Add(child);
        }
    }
}