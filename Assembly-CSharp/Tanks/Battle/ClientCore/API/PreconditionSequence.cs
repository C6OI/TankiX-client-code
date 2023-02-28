namespace Tanks.Battle.ClientCore.API {
    public class PreconditionSequence : CompositeNode {
        public override TreeNodeState Running() {
            for (int i = 0; i < children.Count; i++) {
                BehaviourTreeNode behaviourTreeNode = children[i];
                TreeNodeState treeNodeState = behaviourTreeNode.Update();

                if (treeNodeState != 0) {
                    state = treeNodeState;
                    return state;
                }
            }

            return TreeNodeState.SUCCESS;
        }
    }
}