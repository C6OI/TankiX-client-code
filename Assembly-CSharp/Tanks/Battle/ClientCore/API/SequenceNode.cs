namespace Tanks.Battle.ClientCore.API {
    public class SequenceNode : CompositeNode {
        public override TreeNodeState Running() {
            for (int i = currentChildIndex; i < children.Count; i++) {
                BehaviourTreeNode behaviourTreeNode = children[i];
                TreeNodeState treeNodeState = behaviourTreeNode.Update();

                if (treeNodeState != 0) {
                    state = treeNodeState;
                    currentChildIndex = i;
                    return state;
                }
            }

            return TreeNodeState.SUCCESS;
        }
    }
}