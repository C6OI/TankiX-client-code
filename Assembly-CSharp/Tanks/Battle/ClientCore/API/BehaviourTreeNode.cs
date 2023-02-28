namespace Tanks.Battle.ClientCore.API {
    public abstract class BehaviourTreeNode {
        public TreeNodeState state;

        public abstract void Start();

        public abstract TreeNodeState Running();

        public abstract void End();

        public TreeNodeState Update() {
            if (state != TreeNodeState.RUNNING) {
                Start();
            }

            state = Running();

            if (state != Running()) {
                End();
            }

            return state;
        }

        public void Reset() {
            state = TreeNodeState.NONE;
        }
    }
}