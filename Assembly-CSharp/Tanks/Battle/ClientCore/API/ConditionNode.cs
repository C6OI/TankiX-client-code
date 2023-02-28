using System;

namespace Tanks.Battle.ClientCore.API {
    public class ConditionNode : BehaviourTreeNode {
        public Func<bool> Condition;

        public string Name;

        public ConditionNode(string name = "") => Name = name;

        public override void Start() { }

        public override TreeNodeState Running() {
            if (Condition()) {
                return TreeNodeState.SUCCESS;
            }

            return TreeNodeState.FAILURE;
        }

        public override void End() { }
    }
}