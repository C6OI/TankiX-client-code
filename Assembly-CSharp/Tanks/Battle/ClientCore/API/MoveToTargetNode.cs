using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    public class MoveToTargetNode : BehaviourTreeNode {
        public TankAutopilotControllerSystem.AutopilotTankNode tank;

        float LastMove { get; set; }

        float LastTurn { get; set; }

        public override void Start() { }

        public override TreeNodeState Running() => TreeNodeState.RUNNING;

        public override void End() { }
    }
}