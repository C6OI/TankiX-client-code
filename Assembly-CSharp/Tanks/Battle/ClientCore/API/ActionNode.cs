using System;

namespace Tanks.Battle.ClientCore.API {
    public class ActionNode : BehaviourTreeNode {
        public Action Action;

        public override void Start() { }

        public override TreeNodeState Running() {
            Action();
            return TreeNodeState.SUCCESS;
        }

        public override void End() { }
    }
}