namespace Tanks.Battle.ClientCore.API {
    public abstract class DecoratorNode : BehaviourTreeNode {
        public BehaviourTreeNode Child;

        public void AddChild(BehaviourTreeNode child) {
            Child = child;
        }
    }
}