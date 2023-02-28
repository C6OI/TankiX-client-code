namespace Tanks.Battle.ClientCore.API {
    public class SelectorNode : CompositeNode {
        public override TreeNodeState Running() {
            for (int i = currentChildIndex; i < children.Count; i++) {
                BehaviourTreeNode behaviourTreeNode = children[i];
                TreeNodeState treeNodeState = behaviourTreeNode.Update();

                if (treeNodeState == TreeNodeState.FAILURE) {
                    continue;
                }

                state = treeNodeState;
                currentChildIndex = i;

                for (int j = 0; j < children.Count; j++) {
                    if (j != i) {
                        children[j].Reset();
                    }
                }

                return state;
            }

            return TreeNodeState.FAILURE;
        }
    }
}